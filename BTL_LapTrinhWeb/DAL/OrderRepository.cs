using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LapTrinhWeb.DAL
{
    public class OrderRepository
    {
        public void SaveOrder(Order order)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // ===== INSERT ORDER =====
                    SqlCommand cmd = new SqlCommand("sp_CreateOrder", conn, tran);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", order.UserID);
                    cmd.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                    cmd.Parameters.AddWithValue("@UserName", order.UserName);
                    cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmd.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);
                    cmd.Parameters.AddWithValue("@Address", order.Address);
                    cmd.Parameters.AddWithValue("@City", order.City);
                    cmd.Parameters.AddWithValue("@Postal", order.Postal);

                    int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                    // ===== INSERT DETAILS =====
                    foreach (var item in order.Items)
                    {
                        SqlCommand cmdDetail = new SqlCommand("sp_AddOrderDetail", conn, tran);
                        cmdDetail.CommandType = CommandType.StoredProcedure;

                        cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                        cmdDetail.Parameters.AddWithValue("@ProductID", item.ProductID);
                        cmdDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmdDetail.Parameters.AddWithValue("@Price", item.Price);

                        cmdDetail.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("Lỗi lưu đơn hàng: " + ex.Message);
                }
            }
        }
        public List<Order> GetAllOrders()
        {
            List<Order> list = new List<Order>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                string query = "SELECT * FROM Orders";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new Order
                    {
                        UserID = rd["UserID"] != DBNull.Value ? Convert.ToInt32(rd["UserID"]) : 0,
                        OrderNumber = rd["OrderNumber"]?.ToString(),
                        UserName = rd["UserName"]?.ToString(),
                        TotalAmount = rd["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(rd["TotalAmount"]) : 0,
                        PaymentMethod = rd["PaymentMethod"]?.ToString(),
                        Address = rd["Address"]?.ToString(),
                        City = rd["City"]?.ToString(),
                        Postal = rd["Postal"]?.ToString(),
                        Date = rd["OrderDate"] != DBNull.Value ? Convert.ToDateTime(rd["OrderDate"]) : DateTime.Now
                    });
                }
            }

            return list;
        }
    }
}