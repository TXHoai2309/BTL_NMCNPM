using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class CartService
    {
        private CartManager _manager = new CartManager();

        public List<CartItem> GetCart(int userId)
        {
            return _manager.GetCart(userId);
        }

        public void Add(int userId, int productId, int size, int quantity)
        {
            _manager.Add(userId, productId, size, quantity);
        }

        public void DeleteByProduct(int userId, int productId)
        {
            _manager.DeleteByProduct(userId, productId);
        }
    }
}