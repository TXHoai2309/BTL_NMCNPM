using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.DAL
{
    public class UserRepository
    {
        public User Login(string username, string password)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        Id = (int)reader["UserID"],
                        Username = reader["Username"].ToString(),
                        Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "",
                        Surname = reader["Surname"] != DBNull.Value ? reader["Surname"].ToString() : "",
                        Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                        Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "",
                        QueQuan = reader["QueQuan"] != DBNull.Value ? reader["QueQuan"].ToString() : "",
                        Address1 = reader["Address1"] != DBNull.Value ? reader["Address1"].ToString() : "",
                        Address2 = reader["Address2"] != DBNull.Value ? reader["Address2"].ToString() : "",
                        Postal = reader["Postal"] != DBNull.Value ? reader["Postal"].ToString() : "",
                        City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "",
                        Country = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : ""
                    };
                }
            }

            return null;
        }

        public void Register(User u)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@Password", u.Password);
                cmd.Parameters.AddWithValue("@Surname", u.Surname);
                cmd.Parameters.AddWithValue("@Name", u.Name);
                cmd.Parameters.AddWithValue("@Email", u.Email);
                cmd.Parameters.AddWithValue("@Phone", u.Phone);
                cmd.Parameters.AddWithValue("@QueQuan", u.QueQuan);

                cmd.Parameters.AddWithValue("@Address1", (object)u.Address1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address2", (object)u.Address2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Postal", (object)u.Postal ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@City", (object)u.City ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object)u.Country ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllUsers", conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new User
                    {
                        Id = (int)reader["UserID"],
                        Username = reader["Username"].ToString(),
                        Name = reader["Name"]?.ToString(),
                        Surname = reader["Surname"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        Phone = reader["Phone"]?.ToString()
                    });
                }
            }

            return list;
        }

        public User GetUserByID(int userId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetUserByID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        Id = (int)reader["UserID"],
                        Username = reader["Username"].ToString(),
                        Name = reader["Name"]?.ToString(),
                        Surname = reader["Surname"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        Phone = reader["Phone"]?.ToString(),
                        QueQuan = reader["QueQuan"]?.ToString(),
                        Address1 = reader["Address1"]?.ToString(),
                        Address2 = reader["Address2"]?.ToString(),
                        Postal = reader["Postal"]?.ToString(),
                        City = reader["City"]?.ToString(),
                        Country = reader["Country"]?.ToString()
                    };
                }
            }

            return null;
        }

        public void UpdateUser(User u)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", u.Id);
                cmd.Parameters.AddWithValue("@Surname", u.Surname);
                cmd.Parameters.AddWithValue("@Name", u.Name);
                cmd.Parameters.AddWithValue("@Email", u.Email);
                cmd.Parameters.AddWithValue("@Phone", u.Phone);
                cmd.Parameters.AddWithValue("@QueQuan", u.QueQuan);
                cmd.Parameters.AddWithValue("@Address1", u.Address1);
                cmd.Parameters.AddWithValue("@Address2", u.Address2);
                cmd.Parameters.AddWithValue("@Postal", u.Postal);
                cmd.Parameters.AddWithValue("@City", u.City);
                cmd.Parameters.AddWithValue("@Country", u.Country);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ChangePassword(int userId, string newPassword)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ChangePassword", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsUsernameExists(string username)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CheckUsername", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }
    }
}