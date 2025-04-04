﻿using FoodSelling.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodSelling.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public OrderController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _appDbContext.Orders.Include(o => o.OrderItems)
                                              .ThenInclude(oi => oi.FoodItem)
                                              .OrderByDescending(o => o.OrderDate)
                                              .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _appDbContext.Orders.Include(o => o.OrderItems)
                                              .ThenInclude(oi => oi.FoodItem)
                                              .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

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
