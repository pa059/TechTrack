using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTrack.Api.Data;
using TechTrack.Api.Models;
using TechTrack.Api.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace TechTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
            List<OrderResponseDto> ordersResponse = new List<OrderResponseDto>();
            foreach (var order in orders)
            {
                var response = new OrderResponseDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    ShippingAddress = order.ShippingAddress,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                    }).ToList()
                };
                ordersResponse.Add(response);
            }
            return Ok(ordersResponse);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order= await _context.Orders.
                Include(o => o.OrderItems).ThenInclude(oi=>oi.Product).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return BadRequest("order not exist");
            }

            var response = new OrderResponseDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                ShippingAddress = order.ShippingAddress,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items =order.OrderItems.Select(oi=>new OrderItemResponseDto { 
                    ProductId=oi.ProductId,
                    ProductName=oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                }).ToList()
            };
            return Ok(response);
        }

        [HttpGet("Customer/{id:int}")]
        public async Task<ActionResult<List<OrderResponseDto>>> GetAllOrderOFCustomer(int id)
        {
            var orders = await _context.Orders.
                Include(o => o.OrderItems).ThenInclude(oi => oi.Product).Where(o => o.CustomerId == id).ToListAsync();
            
            List<OrderResponseDto> ordersResponse = new List<OrderResponseDto>();
            foreach (var order in orders)
            {
                var response = new OrderResponseDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    ShippingAddress = order.ShippingAddress,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                    }).ToList()
                };
                ordersResponse.Add(response);
            }
            return Ok(ordersResponse);
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto dto)
        {
            if (dto.CustomerId == null)
            {
                return BadRequest("For now, please provide a registered customer.");
            }

            var customer = await _context.customers.FindAsync(dto.CustomerId);

            if (customer == null)
            {
                return BadRequest("Customer does not exist.");
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                CustomerEmail = customer.Email,
                Customer = customer,
                ShippingAddress = dto.ShippingAddress,
                OrderDate = DateTime.UtcNow,
                Status = "new",
                TotalAmount=0

            };

            foreach (var item in dto.Items)
            {
                var product = await _context.products.FindAsync(item.ProductId);
                if (product == null) {
                    return BadRequest($"For now, product{item.ProductId} does not exist.");
                }

                if (item.Quantity <= 0)
                {
                    return BadRequest("For now, please provide a valid quantity");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    return BadRequest($"For now, not in stock{product.Name}");
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                
                order.TotalAmount+=orderItem.Quantity*orderItem.Price;
                order.OrderItems.Add(orderItem);
                product.StockQuantity -= item.Quantity;

            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            // Create Order next
            return Ok(new 
            {
                order.Id,
                order.CustomerId,
                order.CustomerName,
                order.CustomerEmail,
                order.ShippingAddress,
                order.OrderDate,
                order.Status,
                order.TotalAmount
            });
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = dto.Status;
            
            //_context.customers.Add(customer);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }

   
}
