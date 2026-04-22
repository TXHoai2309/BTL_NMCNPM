using System.Collections.Generic;
using BTL_LapTrinhWeb.DAL;
using BTL_LapTrinhWeb.Models;

namespace BTL_LapTrinhWeb.BLL
{
    public class CartManager
    {
        private CartRepository repo = new CartRepository();

        public void Add(int userId, int productId, int size, int quantity)
        {
            repo.AddToCart(userId, productId, size, quantity);
        }

        public List<CartItem> GetCart(int userId)
        {
            return repo.GetCart(userId);
        }

        public void Update(int cartId, int quantity)
        {
            repo.UpdateCart(cartId, quantity);
        }

        public void Delete(int cartId)
        {
            repo.DeleteCart(cartId);
        }
        public void DeleteByProduct(int userId, int productId)
        {
            repo.DeleteByProduct(userId, productId);
        }
    }
}