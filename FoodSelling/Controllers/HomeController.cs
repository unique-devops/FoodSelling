using FoodSelling.Models;
using FoodSelling.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Admin"))
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

        [HttpPost]
        public IActionResult AddToCart(int foodId)
        {
            _cartService.AddToCart(foodId);
            return RedirectToAction("Index");
        }
    }
}
