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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly CartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, CartService cartService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _cartService = cartService;
            _userManager = userManager;
        }
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);

        //    //var user = await _userManager.GetUserAsync(User); // Use session ID or authenticated user ID
        //    //var cartCount = _cartService.GetCartCount(user.Email);
        //    //ViewData["CartCount"] = cartCount;
        //}

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "AdminDashboard"); // Redirect to Admin dashboard
            }           
            var cartCount = await _cartService.GetCartCount(user?.Email);
            ViewData["CartCount"] = cartCount;
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
        //[HttpPost]
        public async Task<JsonResult> AddToCart(int foodId)
        {
            var user = await _userManager.GetUserAsync(User);
            var food = await _appDbContext.Foods.FindAsync(foodId);
            if (food == null) return Json(new { message = "failed" });
            await _cartService.AddToCart(new CartItem { UserId = user.Email, ProductId = food.Id, Price = food.Price, ProductName = food.Name, Quantity = 1 });
            return Json(new { message = "success" });

        }
    }
}
