namespace TechTrack.Web.Models
{
    public class CartItem
    {
        public ProductDto Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
