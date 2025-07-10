using FoodSelling.Models;

namespace FoodSelling.ViewModels
{
    public class CheckoutViewModel
    {
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
