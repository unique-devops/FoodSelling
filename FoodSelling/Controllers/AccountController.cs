using FoodSelling.Models;
using FoodSelling.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodSelling.Controllers
{
    public class AccountController : BaseController
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //}
       
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager): base(userManager:userManager,signInManager:signInManager,roleManager:roleManager)
        {
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {      
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign role
                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                            return Json(new { success = true });
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
