using BTL_LapTrinhWeb.BLL;
using System;
using System.Data;

namespace BTL_LapTrinhWeb.Services
{
    public class OrderService
    {
        private OrderBLL _orderBLL;

        public OrderService()
        {
            _orderBLL = new OrderBLL();
        }

        // =====================================
        // 1. LẤY DANH SÁCH ĐƠN HÀNG
        // =====================================
        public DataTable GetOrdersByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new DataTable();

            return _orderBLL.GetOrders(userId);
        }

        // =====================================
        // 2. LẤY CHI TIẾT ĐƠN HÀNG
        // =====================================
        public DataSet GetOrderDetail(int orderId)
        {
            if (orderId <= 0)
                return null;

            return _orderBLL.GetOrderDetail(orderId);
        }

        // =====================================
        // 3. CHECK QUYỀN (BẢO MẬT)
        // =====================================
        public bool IsValidUserOrder(int orderId, string userId)
        {
            return _orderBLL.IsOrderBelongToUser(orderId, userId);
        }

        // =====================================
        // 4. FORMAT STATUS
        // =====================================
        public string GetStatusText(string status)
        {
            return _orderBLL.FormatStatus(status);
        }

        // =====================================
        // 5. HELPER: GET TOTAL ORDER COUNT
        // =====================================
        public int CountOrders(string userId)
        {
            return _orderBLL.CountOrders(userId);
        }
    }
}