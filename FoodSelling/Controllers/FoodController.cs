using FoodSelling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodSelling.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodController : BaseController
    {       

        public FoodController(AppDbContext appDbContext) :base(appDbContext:appDbContext)
        {           
        }
       
        public async Task<IActionResult> Index()
        {
            var orders = await _appDbContext.Foods.Include(o => o.Category)                                             
                                              .OrderByDescending(o => o.Name)
                                              .ToListAsync();
            return View(orders);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var order = await _appDbContext.Foods.Include(o => o.Category)                                              
                                              .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
       
    }
}
