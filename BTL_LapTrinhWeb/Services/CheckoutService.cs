using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;
using System.Collections.Generic;

namespace BTL_LapTrinhWeb.Services
{
    public class CheckoutService
    {
        private CheckoutManager _manager = new CheckoutManager();

        public Order ProcessOrder(User user, List<CartItem> cart,
            string payment, string address, string city, string postal)
        {
            var order = _manager.CreateOrder(user, cart, payment, address, city, postal);
            _manager.SaveOrder(order);
            return order;
        }

        public decimal GetTotal(List<CartItem> cart)
        {
            return _manager.CalculateTotal(cart);
        }
    }
}