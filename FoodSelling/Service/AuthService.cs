using FoodSelling.Common;
using FoodSelling.Models;
using System.Security.Cryptography;
using System.Text;

namespace FoodSelling.Service
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _appDbContext;       
        public AuthService(IHttpContextAccessor httpContextAccessor,AppDbContext appDbContext) {
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
            //_users.Add(new User { Id=1, User_Name="admin",Email="admin@gmail.com",PasswordHash =HashPassword("admin@123"),Role="Admin", Active="active"});
            //_users.Add(new User { Id=2, User_Name="user",Email="user@gmail.com",PasswordHash =HashPassword("user@123"),Role="Customer", Active="active"});
        }

        public async Task<DbResult> Register(string email,string password,string role)
        {

            try
            {
                if (_appDbContext.Users.Any(u => u.Email == email)) return DbResult.FailureResult("Username already exists");
                var user = new User
                {
                    Email = email,
                    PasswordHash = HashPassword(password),
                    Role = role,
                    Active = "Active"
                };
                _appDbContext.Users.Add(user);
                int rowsAffected = await _appDbContext.SaveChangesAsync();
                return rowsAffected > 0 ? DbResult.SuccessResult("Register successfully!") : DbResult.FailureResult("Failed");
            }
            catch(Exception ex) {
                return DbResult.FailureResult(ex.Message);
            }
        }

        public User Login(string email,string password)
        { 
            var user = _appDbContext.Users.FirstOrDefault(u=>u.Email ==email);
            if (user != null && VerifyPassword(password,user.PasswordHash))
            {
                return user;
            }
            return null;            
        }

        private string HashPassword(string password)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
        public bool IsUserAuthenticated()
        {
            var userId = _httpContextAccessor?.HttpContext?.Session.GetInt32("userid");
            return userId != null;
        }
        public int? GetUserId()
        {
            return _httpContextAccessor?.HttpContext?.Session.GetInt32("userid");
        }

        // Get the user role from the session
        public string GetUserRole()
        {
            return _httpContextAccessor?.HttpContext?.Session.GetString("role");
        }
    }
}
