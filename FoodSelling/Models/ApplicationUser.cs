using Microsoft.AspNetCore.Identity;

namespace FoodSelling.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; } = "";
        public string? ProfileImage { get; set; }  // Path to the profile image
        public DateTime? DateOfBirth { get; set; }
    }
}
