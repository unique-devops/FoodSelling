using Microsoft.AspNetCore.Http;

namespace FoodSelling.Service
{
    public class CartService
    {
        private readonly ISession _session;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void AddToCart(int foodId)
        {
            //var cart = GetCart();
            //if (cart.ContainsKey(foodId))
            //{
            //    cart[foodId]++;
            //}
            //else
            //{
            //    cart[foodId] = 1;
            //}
            //SaveCart(cart);
        }

        //public Dictionary<int, int> GetCart()
        //{
        //    var cart = _session.Get<Dictionary<int, int>>("cart");
        //    return cart ?? new Dictionary<int, int>();
        //}

        //private void SaveCart(Dictionary<int, int> cart)
        //{
        //    _session.Set("cart", cart);
        //}
    }
}
