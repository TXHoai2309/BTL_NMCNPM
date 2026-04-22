using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BTL_LapTrinhWeb.DAL
{
    public class ProductRepository
    {
        // ================= GET ALL (JOIN CATEGORY) =================
        public List<Products> GetAllProducts()
        {
            List<Products> list = new List<Products>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                string query = @"
                    SELECT p.*, c.CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(MapProduct(reader));
                }
            }

            return list;
        }

        // ================= GET BY ID =================
        public Products GetProductByID(int id)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                string query = @"
                    SELECT p.*, c.CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                    WHERE p.ProductID = @ProductID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return MapProduct(reader);
                }
            }

            return null;
        }

        // ================= INSERT =================
        public void InsertProduct(Products p)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.Parameters.AddWithValue("@Description", p.Description);
                cmd.Parameters.AddWithValue("@Image", p.Image);
                cmd.Parameters.AddWithValue("@Stock", p.Stock);
                cmd.Parameters.AddWithValue("@CategoryID", p.CategoryID);
                cmd.Parameters.AddWithValue("@Image1", p.Image1);
                cmd.Parameters.AddWithValue("@Image2", p.Image2);
                cmd.Parameters.AddWithValue("@Image3", p.Image3);
                cmd.Parameters.AddWithValue("@Image4", p.Image4);
                cmd.Parameters.AddWithValue("@Gender", p.Gender);
                cmd.Parameters.AddWithValue("@Size", p.Size);
                cmd.Parameters.AddWithValue("@SalePercent", p.SalePercent);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ================= UPDATE =================
        public void UpdateProduct(Products p)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateProductFull", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", p.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.Parameters.AddWithValue("@Description", p.Description);
                cmd.Parameters.AddWithValue("@Image", p.Image);
                cmd.Parameters.AddWithValue("@Stock", p.Stock);
                cmd.Parameters.AddWithValue("@CategoryID", p.CategoryID);
                cmd.Parameters.AddWithValue("@Image1", p.Image1);
                cmd.Parameters.AddWithValue("@Image2", p.Image2);
                cmd.Parameters.AddWithValue("@Image3", p.Image3);
                cmd.Parameters.AddWithValue("@Image4", p.Image4);
                cmd.Parameters.AddWithValue("@Gender", p.Gender);
                cmd.Parameters.AddWithValue("@Size", p.Size);
                cmd.Parameters.AddWithValue("@SalePercent", p.SalePercent);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ================= DELETE =================
        public void DeleteProduct(int id)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ================= MAP DATA =================
        private Products MapProduct(SqlDataReader reader)
        {
            return new Products
            {
                ProductID = (int)reader["ProductID"],
                ProductName = reader["ProductName"].ToString(),
                Price = (decimal)reader["Price"],
                Description = reader["Description"]?.ToString(),
                Image = reader["Image"]?.ToString(),
                Stock = reader["Stock"] != DBNull.Value ? (int)reader["Stock"] : 0,

                CategoryID = (int)reader["CategoryID"],
                CategoryName = reader["CategoryName"]?.ToString(), // 🔥 QUAN TRỌNG

                Gender = reader["Gender"]?.ToString(),
                Size = reader["Size"]?.ToString(),

                SalePercent = reader["SalePercent"] != DBNull.Value
                    ? (decimal)reader["SalePercent"]
                    : 0,

                Image1 = reader["Image1"]?.ToString(),
                Image2 = reader["Image2"]?.ToString(),
                Image3 = reader["Image3"]?.ToString(),
                Image4 = reader["Image4"]?.ToString(),

                CreatedAt = reader["CreatedAt"] != DBNull.Value
                    ? (DateTime?)reader["CreatedAt"]
                    : null
            };
        }
    }
}