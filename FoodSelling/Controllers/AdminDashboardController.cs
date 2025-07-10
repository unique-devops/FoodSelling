using FoodSelling.Models;
using FoodSelling.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace FoodSelling.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : BaseController
    {      
        public AdminDashboardController(AppDbContext appDbContext) :base(appDbContext:appDbContext)
        {           
        }
        [Authorize(Roles = "Admin")] // Only accessible by Admin role
        public async Task<IActionResult> Index()
        {
            var foodItems = await _appDbContext.Foods.ToListAsync();
            return View(foodItems);
        }
       
        public async Task<IActionResult> AddCategory()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            var model = new CategoryViewModel
            {
                Categories = categories
            };
            return View(model);           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _appDbContext.Categories.AddAsync(model.Category);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(AddCategory)); // Redirect after successful addition
            }
            model.Categories = await _appDbContext.Categories.ToListAsync();
            return View(model); // If validation fails, return with errors
        }

        public async Task<IActionResult> AddFood()
        {
            // Pass categories to the view for dropdown selection
            ViewData["CategoryId"] = new SelectList(await _appDbContext.Categories.ToListAsync(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFood(Food food, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        food.ImageUrl = Convert.ToBase64String(imageBytes); // Convert to Base64
                    }
                }
                await _appDbContext.Foods.AddAsync(food);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(AddFood)); // Redirect after successful addition
            }

            // If validation fails, repopulate categories for dropdown and return with errors
            ViewData["CategoryId"] = new SelectList(await _appDbContext.Categories.ToListAsync(), "Id", "Title", food.CategoryId);
            return View(food);
        }

    }
}
