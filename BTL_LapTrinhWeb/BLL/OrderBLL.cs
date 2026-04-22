using BTL_LapTrinhWeb.DAL;
using System;
using System.Data;

namespace BTL_LapTrinhWeb.BLL
{
    public class OrderBLL
    {
        private OrderDAL _orderDAL;

        public OrderBLL()
        {
            _orderDAL = new OrderDAL();
        }

        // =========================================
        // 1. LẤY DANH SÁCH ĐƠN HÀNG THEO USER
        // =========================================
        public DataTable GetOrders(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new DataTable();

            DataTable dt = _orderDAL.GetOrdersByUser(userId);

            // Business logic: format dữ liệu hiển thị
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Date", typeof(string));
                dt.Columns.Add("Total", typeof(string));
                dt.Columns.Add("Items", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    // Format Date
                    DateTime date;
                    if (DateTime.TryParse(row["OrderDate"].ToString(), out date))
                        row["Date"] = date.ToString("dd/MM/yyyy");

                    // Format Total
                    decimal total;
                    if (decimal.TryParse(row["TotalAmount"].ToString(), out total))
                        row["Total"] = "$" + total.ToString("N2");

                    // Demo Items (bạn có thể thay bằng count thật)
                    row["Items"] = "View detail";
                }
            }

            return dt;
        }

        // =========================================
        // 2. LẤY CHI TIẾT ĐƠN HÀNG
        // =========================================
        public DataSet GetOrderDetail(int orderId)
        {
            if (orderId <= 0)
                return null;

            DataSet ds = _orderDAL.GetOrderDetail(orderId);

            if (ds == null || ds.Tables.Count < 2)
                return null;

            // Business logic có thể thêm ở đây nếu cần
            return ds;
        }

        // =========================================
        // 3. KIỂM TRA ĐƠN HÀNG CÓ THUỘC USER KHÔNG
        // (BẢO MẬT - NÊN CÓ)
        // =========================================
        public bool IsOrderBelongToUser(int orderId, string userId)
        {
            if (orderId <= 0 || string.IsNullOrEmpty(userId))
                return false;

            DataSet ds = _orderDAL.GetOrderDetail(orderId);

            if (ds == null || ds.Tables.Count == 0)
                return false;

            DataTable orderTable = ds.Tables[0];

            if (orderTable.Rows.Count == 0)
                return false;

            string dbUserId = orderTable.Rows[0]["UserID"].ToString();

            return dbUserId == userId;
        }

        // =========================================
        // 4. FORMAT STATUS (DÙNG CHUNG)
        // =========================================
        public string FormatStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
                return "";

            switch (status.ToLower())
            {
                case "pending":
                    return "Pending";

                case "processing":
                    return "Processing";

                case "shipping":
                    return "Shipping";

                case "delivered":
                    return "Delivered";

                case "cancelled":
                    return "Cancelled";

                default:
                    return status;
            }
        }

        // =========================================
        // 5. ĐẾM SỐ ĐƠN HÀNG
        // =========================================
        public int CountOrders(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return 0;

            DataTable dt = _orderDAL.GetOrdersByUser(userId);

            return dt != null ? dt.Rows.Count : 0;
        }
    }
}