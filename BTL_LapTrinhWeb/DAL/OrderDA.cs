using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BTL_LapTrinhWeb.DAL
{
    public class OrderDAL
    {
        private string conn = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

        // =============================
        // 1. LẤY DANH SÁCH ĐƠN
        // =============================
        public DataTable GetOrdersByUser(string userId)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("sp_GetOrdersByUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        // =============================
        // 2. LẤY CHI TIẾT ĐƠN
        // =============================
        public DataSet GetOrderDetail(int orderId)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("sp_GetOrderDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;
            }
        }
    }
}