using FoodSelling.Models;
using FoodSelling.SignalRHub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FoodSelling.Service
{
    public class CartService
    {     
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<CartHub> _hubContext;

        public CartService(AppDbContext appDbContext, IHubContext<CartHub> hubContext)
        {          
            _appDbContext = appDbContext;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            return await _appDbContext.CartItems.ToListAsync();
        }
        public async Task AddToCart(CartItem cartItem)
        {            
            await _appDbContext.CartItems.AddAsync(cartItem);            
            await _appDbContext.SaveChangesAsync();
            int cartCount = await  GetCartCount(cartItem.UserId);
            await _hubContext.Clients.All.SendAsync("ReceiveCartCount", cartCount);
        }
        public async Task<int> UpdateCart(string userId, int productId, int quantity)
        {           
            var cartItem = await _appDbContext.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                _appDbContext.CartItems.Update(cartItem);
                return await _appDbContext.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> RemoveFromCart(string userId, int foodId)
        {
            var cartItem = await _appDbContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == foodId);
            if (cartItem != null)
            {
                _appDbContext.CartItems.Remove(cartItem);
               return await _appDbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ClearCart(string userId)
        {
            var cartItem = await _appDbContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartItem != null)
            {
                _appDbContext.CartItems.RemoveRange(_appDbContext.CartItems.Where(c => c.UserId == userId));
                return await _appDbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<List<CartItem>> GetCart(string userId)
        {            
            return await _appDbContext.CartItems.Where(c=>c.UserId == userId).ToListAsync();
        }
        public async Task<int> GetCartCount(string userId)
        {
            return await _appDbContext.CartItems.CountAsync(c => c.UserId == userId);
        }
    }
}
