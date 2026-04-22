using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class CartService
    {
        private CartManager _manager = new CartManager();

        public List<CartItem> GetCart()
        {
            return _manager.GetCart();
        }

        public void RemoveItem(string productNumber)
        {
            _manager.RemoveItem(productNumber);
        }

        public decimal GetTotal(List<CartItem> cart)
        {
            return _manager.GetTotal(cart);
        }
    }
}