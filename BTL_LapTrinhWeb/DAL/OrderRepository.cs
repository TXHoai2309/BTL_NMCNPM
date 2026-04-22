using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace BTL_LapTrinhWeb.DAL
{
    public class OrderRepository
    {
        public void SaveOrder(Order order)
        {
            var history = HttpContext.Current.Application["OrderHistory"] as List<Order> ?? new List<Order>();

            lock (HttpContext.Current.Application)
            {
                history.Add(order);
                HttpContext.Current.Application["OrderHistory"] = history;
            }
        }
        public List<Order> GetAllOrders()
        {
            var orders = HttpContext.Current.Application["OrderHistory"] as List<Order>;
            return orders ?? new List<Order>();
        }
    }
}