using FoodSelling.Models;
using FoodSelling.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FoodSelling.Controllers
{
    public class HomeController : BaseController
    {
              
        public HomeController(
            AppDbContext appDbContext,
            CartService cartService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
            : base(appDbContext, cartService, userManager, signInManager, roleManager)
        {                       
        }       

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "AdminDashboard"); // Redirect to Admin dashboard
            }
            var foodItems = await _appDbContext.Foods.ToListAsync();
            return View(foodItems);
        }             

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //[HttpPost]
        //public async Task<IActionResult> AddToCart(int foodId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var food = await _appDbContext.Foods.FindAsync(foodId);
        //    if (food != null)
        //    {
        //        await _cartService.AddToCart(new CartItem { UserId = user.Email, ProductId = food.Id, Price = food.Price, ProductName = food.Name, Quantity = 1 });
        //    }
        //    return Content("success");

        //}
        
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddToCart(int foodId)
        {
            var user = await _userManager.GetUserAsync(User);
            var food = await _appDbContext.Foods.FindAsync(foodId);
            if (food == null) return Json(new { message = "failed" });
            var isAlreadyAdded = await _appDbContext.CartItems.Where(c => c.UserId == user.Email && c.ProductId == foodId).FirstOrDefaultAsync();
            if (isAlreadyAdded != null)
            {
                return Json(new { message = "Already added in cart." });
            }
            else
            {
                await _cartService.AddToCart(new CartItem { UserId = user.Email, ProductId = food.Id, Price = food.Price, ProductName = food.Name, Quantity = 1 });
                return Json(new { message = "success" });
            }
            

        }
    }
}
