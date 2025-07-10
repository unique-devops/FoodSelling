using FoodSelling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodSelling.Controllers
{
    public class OrderController : BaseController
    {       

        public OrderController(AppDbContext appDbContext) :base(appDbContext:appDbContext)
        {           
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var orders = await _appDbContext.Orders.Include(o => o.OrderItems)
                                              .ThenInclude(oi => oi.FoodItem)
                                              .OrderByDescending(o => o.OrderDate)
                                              .ToListAsync();
            return View(orders);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _appDbContext.Orders.Include(o => o.OrderItems)
                                              .ThenInclude(oi => oi.FoodItem)
                                              .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var order = await _appDbContext.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                _appDbContext.Update(order);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
