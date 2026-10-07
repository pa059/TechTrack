using TechTrack.Web.Models;

namespace TechTrack.Web.Services
{
    public class CartService
    {
        public List<CartItem> CartItems { get; set; } = new();
        public event Action? OnChange;

        public decimal CartPrice { get; set; }
        public void AddToCart(ProductDto product)
        {
            var existingitem = CartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existingitem == null)
            {
                CartItem item = new CartItem();
                item.Product = product;
                item.Quantity = 1;
                CartItems.Add(item);
            }
            else
            {
                existingitem.Quantity++;
            }
            //quantity++;
            //lastAddedProduct = product.Name;
            Console.WriteLine($"added:{product.Name}");
            NotifyStateChanged();
        }

        public decimal GetPrice()
        {
            decimal subtotal = 0;
            foreach (var item in CartItems)
            {
                 subtotal= subtotal+(item.Quantity*item.Product.Price);
            }

            //CartPrice+= subtotal;
            return subtotal;
        }

        public void IncreaseQuantity(CartItem item)
        {
            item.Quantity++;
            NotifyStateChanged();
        }

        public void DecreaseQuantity(CartItem item)
        {
            if (item.Quantity > 1) {
                item.Quantity--;
            } 
            else
            {
                CartItems.Remove(item);
            }
            NotifyStateChanged();
        }

        public int GetCartQuantity()
        {
            int totalQuantity=0;
            foreach(var item in CartItems)
            {
                totalQuantity += item.Quantity;
            }
            
            return totalQuantity;
            
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
