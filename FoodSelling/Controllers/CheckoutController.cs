using FoodSelling.Models;
using FoodSelling.Service;
using FoodSelling.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSelling.Controllers
{
    [Authorize]
    public class CheckoutController : BaseController
    {
        public CheckoutController(AppDbContext appDbContext,CartService cartService):base(appDbContext:appDbContext,cartService:cartService)
        {
            
        }
        public async Task<IActionResult> Index()
        {
            var cartItem = await _cartService.GetCartItems();
            var totalAmount = cartItem.Sum(c => c.Price * c.Quantity);
            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItem.ToList(),
                TotalAmount = totalAmount,
            };
            return View(checkoutViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PlaceOrder(CheckoutViewModel checkoutViewModel)
        {
            var cartItem = await _cartService.GetCartItems();
            var totalAmount = cartItem.Sum(c => c.Price * c.Quantity);
            checkoutViewModel.CartItems = cartItem.ToList();
            checkoutViewModel.TotalAmount = totalAmount;
            if (!ModelState.IsValid)
            {
                return View("Index", checkoutViewModel);
            }
           
            var order = new Order
            {
                CustomerEmail = User.Identity.Name,
                CustomerName = User.Identity.Name,
                Quantity = cartItem.Sum(c=>c.Quantity),
                Status = "Order Confirmed",
                OrderDate = DateTime.Now,
                Address = checkoutViewModel.ShippingAddress,
                TotalAmount = totalAmount,
                OrderItems = cartItem.Select(item => new OrderItem
                {
                    FoodItemId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
           
            using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                try {
                    await _appDbContext.Orders.AddAsync(order);
                    await _appDbContext.SaveChangesAsync();                  
                    await transaction.CommitAsync();
                    await _cartService.ClearCart(User.Identity.Name);
                    return RedirectToAction("Index","Order");
                    //return RedirectToAction("Confirmation", new { orderId = order.OrderId });
                }
                catch
                {
                    transaction.Rollback();
                    // Log the exception for debugging
                    // For example, you can use a logging framework like NLog, Serilog, or log to a file.
                    //throw new Exception("An error occurred while processing the order. The transaction has been rolled back.", ex);
                    ModelState.AddModelError("", "An error occurred while placing the order. Please try again later.");
                    return View("Index", checkoutViewModel);
                }
            }                                                         
        }

        public ActionResult Confirmation(int orderId)
        {
            var order = _appDbContext.Orders.Find(orderId);
            if (order == null)
            {
                return View(order);
            }

            return View(order);
        }
    }
}
