using FoodSelling.Models;
using FoodSelling.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodSelling.Controllers
{
    public class BaseController : Controller
    {       
        protected readonly AppDbContext _appDbContext;
        protected readonly CartService _cartService;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        public BaseController(        
        AppDbContext appDbContext = null,
        CartService cartService = null,
        UserManager<ApplicationUser> userManager = null,
        SignInManager<ApplicationUser> signInManager =null,
        RoleManager<IdentityRole> roleManager =null)
        {            
            _appDbContext = appDbContext;
            _cartService = cartService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;            
        }
       
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //CartManage();

            base.OnActionExecuted(context);
        }
        private async void CartManage()
        {
            if (User != null && _cartService != null && _userManager != null)
            {
                var cartCount = await _cartService.GetCartCount(User.Identity.Name);
                ViewData["CartCount"] = cartCount;
            }
            else
            {
                ViewData["CartCount"] = 0; // User is not authenticated, set cart count to 0
            }          
           
        }

    }
}
