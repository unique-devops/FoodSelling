using FoodSelling.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodSelling.Service
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                var roleExist =  await roleManager.RoleExistsAsync(role);
                if (!roleExist) {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminUser == null) {
                var user = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.com"                    
                };

                var result = await userManager.CreateAsync(user,"Admin@123");

                if (result.Succeeded)
                { 
                    await userManager.AddToRoleAsync(user,"Admin");
                }
            }

        }
    }
}
