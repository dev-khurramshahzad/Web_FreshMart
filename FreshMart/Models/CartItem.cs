using FreshMart.Models;

namespace FreshMart.Models
{
    public class CartItem
    {
        public Item? item { get; set; }
        public int quantity { get; set; }
    }
}
