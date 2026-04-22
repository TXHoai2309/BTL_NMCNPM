using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class CartManager
    {
        private CartRepository _repo = new CartRepository();

        public List<CartItem> GetCart()
        {
            return _repo.GetCart();
        }

        public void RemoveItem(string productNumber)
        {
            var cart = _repo.GetCart();

            var item = cart.FirstOrDefault(c => c.ProductNumber == productNumber);
            if (item != null)
            {
                cart.Remove(item);
                _repo.SaveCart(cart);
            }
        }

        public decimal GetTotal(List<CartItem> cart)
        {
            return cart.Sum(i => i.LineTotal);
        }
    }
}