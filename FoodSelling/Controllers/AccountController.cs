using FoodSelling.Models;
using FoodSelling.Service;
using Microsoft.AspNetCore.Mvc;

namespace FoodSelling.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        public AccountController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _authService.Login(email,password);
            if (user != null)
            {
                //TempData["UserId"] = user.Id;
                //TempData["Role"] = user.Role;
                HttpContext.Session.SetInt32("userid", user.Id);
                HttpContext.Session.SetString("role", user.Role);
                return RedirectToAction("Index","Home");
            }
            ViewBag.Error = "Invalid login credentials";
            return View();
        }
        public IActionResult Register()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            //if (!ModelState.IsValid)
            //{
            //    // If validation fails, return bad request with validation errors
            //    return BadRequest(ModelState);
            //}
            var result = await _authService.Register(email, password, "Customer");
            if (result.Success)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Error = result.Message;
            return View();
        }

        public IActionResult Logout() {
            //TempData.Clear();
            HttpContext.Session.Clear();            
            return RedirectToAction("Login");
        }
    }
}
