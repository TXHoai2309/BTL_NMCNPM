using System.Collections.Generic;
using System.Web;
using BTL_LapTrinhWeb.Models;

namespace BTL_LapTrinhWeb.DAL
{
    public class CartRepository
    {
        public List<CartItem> GetCart()
        {
            return HttpContext.Current.Session["ShoppingCart"] as List<CartItem>
                   ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
        {
            HttpContext.Current.Session["ShoppingCart"] = cart;
        }
    }
}