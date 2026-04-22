using System;
using System.Collections.Generic;

namespace BTL_LapTrinhWeb.Models
{
    public class Order
    {
        public int UserID { get; set; } // 🔥 THÊM

        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }

        public List<CartItem> Items { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string PaymentMethod { get; set; }
    }
}