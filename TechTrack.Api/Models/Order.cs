using Microsoft.EntityFrameworkCore;

namespace TechTrack.Api.Models

{
    public class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        [Precision(10, 2)]
        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
