using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTrack.Api.Data;
using TechTrack.Api.Models;
namespace TechTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private static readonly List<Customer> customers =
        [
            new Customer
            {
                Id = 1,
                Name = "Anna",
                Email = "anna@example.com"
            },

            new Customer
            {
                Id = 2,
                Name = "David",
                Email = "david@example.com"
            }
        ];

        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context= context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            var customers = await _context.customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>>  GetCustomer(int id) {
            //var customer = customers.FirstOrDefault(customer => customer.Id == id);
            var customer = await _context.customers.FindAsync(id);
            if (customer == null) {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            _context.customers.Add(customer);
            await _context.SaveChangesAsync();
            //customer.Id = customers.Count+1;
            //customers.Add(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id =customer.Id}, customer);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, Customer updatedcustomer)
        {
            var customer = await _context.customers.FindAsync(id);
            if (customer == null) {
                return NotFound();
            }
            customer.Name = updatedcustomer.Name;
            customer.Email = updatedcustomer.Email;
            //_context.customers.Add(customer);
           await _context.SaveChangesAsync();
           return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            _context.customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public List<Customer> GetCustomers()
        //{
        //    return new List<Customer>
        //{
        //    new Customer
        //    {
        //        Id = 1,
        //        Name = "Anna",
        //        Email = "anna@example.com"
        //    },

        //    new Customer
        //    {
        //        Id = 2,
        //        Name = "David",
        //        Email = "david@example.com"
        //    }
        //};
        //}
    }
}
