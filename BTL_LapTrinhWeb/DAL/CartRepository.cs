using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BTL_LapTrinhWeb.Models;

namespace BTL_LapTrinhWeb.DAL
{
    public class CartRepository
    {
        public void AddToCart(int userId, int productId, int size, int quantity)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AddToCart", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Size", size);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<CartItem> GetCart(int userId)
        {
            List<CartItem> list = new List<CartItem>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetCartByUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new CartItem
                    {
                        CartID = Convert.ToInt32(rd["CartID"]),
                        Name = rd["ProductName"].ToString(),
                        Price = Convert.ToDecimal(rd["Price"]),
                        Sale = rd["SalePercent"] != DBNull.Value ? Convert.ToInt32(rd["SalePercent"]) : 0,
                        Size = rd["Size"] != DBNull.Value ? rd["Size"].ToString() : "0",
                        Quantity = Convert.ToInt32(rd["Quantity"]),
                        ImageUrl = rd["Image"].ToString()
                    });
                }
            }

            return list;
        }

        public void UpdateCart(int cartId, int quantity)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateCart", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CartID", cartId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCart(int cartId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteCart", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CartID", cartId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteByProduct(int userId, int productId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID",
                    conn);

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}