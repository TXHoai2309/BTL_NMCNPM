using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class UserService
    {
        private UserManager _manager = new UserManager();

        // ================= LOGIN =================
        public User Login(string username, string password)
        {
            // admin hard-code
            if (username.Equals("admin", System.StringComparison.OrdinalIgnoreCase)
                && password == "12345678")
            {
                return new User { Username = "admin", Name = "Administrator" };
            }

            //
            return _manager.ValidateUser(username, password);
        }

        // ================= CHECK USERNAME =================
        public bool IsUsernameExists(string username)
        {
            return _manager.IsUsernameExists(username);
        }

        // ================= REGISTER =================
        public bool Register(User user)
        {
            if (IsUsernameExists(user.Username))
                return false;

            _manager.Register(user);
            return true;
        }

        // ================= GET USER =================
        public User GetUser(int id)
        {
            return _manager.GetUserById(id);
        }

        // ================= UPDATE PROFILE =================
        public void UpdateUser(User user)
        {
            _manager.UpdateUser(user);
        }

        // ================= ORDER HISTORY =================
        public List<Order> GetOrders(string username)
        {
            return _manager.GetUserOrders(username);
        }
    }
}