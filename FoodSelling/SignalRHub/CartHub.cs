using Microsoft.AspNetCore.SignalR;

namespace FoodSelling.SignalRHub
{
    public class CartHub : Hub
    {
        public async Task UpdateCartCount(int cartCount)
        {
            await Clients.All.SendAsync("ReceiveCartCount", cartCount);
        }
    }
}
