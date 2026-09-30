using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTrack.Api.Data;
using TechTrack.Api.Models;

namespace TechTrack.Api.Controllers
{   [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase
    {
       private readonly AppDbContext _context;
        public ProductsController(AppDbContext context) { 
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //var customer = customers.FirstOrDefault(customer => customer.Id == id);
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.products.Add(product);
            await _context.SaveChangesAsync();
            //customer.Id = customers.Count+1;
            //customers.Add(customer);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateCustomer(int id, Product updatedproduct)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = updatedproduct.Name;
            product.Description = updatedproduct.Description;
            product.Price = updatedproduct.Price;
            product.Category = updatedproduct.Category;
            product.StockQuantity = updatedproduct.StockQuantity;
            
            //_context.customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = updatedproduct.Id }, product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteCustomer(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
