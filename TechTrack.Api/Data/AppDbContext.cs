using Microsoft.EntityFrameworkCore;
using TechTrack.Api.Models;

namespace TechTrack.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { }

        public DbSet<Customer> customers { get; set; }

        public DbSet<Product> products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
