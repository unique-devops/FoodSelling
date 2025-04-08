using FoodSelling.Service;
using Microsoft.AspNetCore.Mvc;

namespace FoodSelling.ViewComponents
{
    
    public class CartViewComponent : ViewComponent
    {
        private readonly CartService _cartService;
        public CartViewComponent(CartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string userEmail)
        {
            // Get the cart count for the given user (you can replace this with real cart count logic)
            var cartCount = await _cartService.GetCartCount(userEmail);

            // Return the view with the cart count data
            return View(cartCount);
        }
    }

    
}
