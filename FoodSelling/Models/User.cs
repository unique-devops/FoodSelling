using System.ComponentModel.DataAnnotations;

namespace FoodSelling.Models
{
    public class User
    {
        public int Id { get; set; }

        //[Required(ErrorMessage ="Username is required.")]
        //[StringLength(50,ErrorMessage ="Username cannot be longer than 50 characters.")]
        public string User_Name { get; set; } = "";
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(256, ErrorMessage = "Password hash cannot be longer than 256 characters.")]
        public string PasswordHash { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "City cannot be longer than 100 characters.")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "State cannot be longer than 100 characters.")]
        public string? State { get; set; }

        [StringLength(10, ErrorMessage = "Pincode cannot be longer than 10 characters.")]
        public string? Pincode { get; set; }

        [StringLength(10, ErrorMessage = "Active field cannot be longer than 10 characters.")]
        public string Active { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot be longer than 50 characters.")]
        public string Role { get; set; }
      
        public string? photo { get; set; }
    }
}
