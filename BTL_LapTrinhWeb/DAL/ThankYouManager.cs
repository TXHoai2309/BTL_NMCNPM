using System;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class ThankYouManager
    {
        private OrderRepository _repo = new OrderRepository();

        public Order GetLastOrder(string orderCode, string username)
        {
            var orders = _repo.GetAllOrders();

            return orders.LastOrDefault(o =>
                o.OrderNumber == orderCode &&
                o.UserName == username);
        }
    }
}