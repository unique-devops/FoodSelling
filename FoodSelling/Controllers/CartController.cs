using FoodSelling.Models;
using FoodSelling.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodSelling.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {       

        public CartController(CartService cartService) : base(cartService: cartService) 
        {            
        }
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartItems();
            return View(cart);
        }
       
        public async Task<IActionResult> RemoveFromCart(int foodId)
        {
            await _cartService.RemoveFromCart(User.Identity.Name,foodId);
            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> UpdateCart(int foodId,int qty)
        {
            if (qty < 0)
            {
                qty = -1 * (qty + 1);
            }
            else
            {
                qty = qty + 1;
            }            
            await _cartService.UpdateCart(User.Identity.Name, foodId,qty);
            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCart(User.Identity.Name);
            return RedirectToAction("Index");
        }       
    }
}
