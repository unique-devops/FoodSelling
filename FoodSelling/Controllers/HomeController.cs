using FoodSelling.Models;
using FoodSelling.Service;
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
        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, CartService cartService)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
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
