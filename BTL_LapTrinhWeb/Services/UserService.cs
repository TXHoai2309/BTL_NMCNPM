using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class UserService
    {
        private UserManager _manager = new UserManager();

        public User Login(string username, string password)
        {
            if (username.ToLower() == "admin" && password == "12345678")
            {
                return new User
                {
                    Username = "admin",
                    Name = "Admin"
                };
            }

            return _manager.Login(username, password);
        }

        public bool Register(User user)
        {
            if (_manager.IsUsernameExists(user.Username))
                return false;

            _manager.Register(user);
            return true;
        }
        public User GetUser(int id)
        {
            return _manager.GetUserByID(id);
        }
        public void UpdateUser(User user)
        {
            _manager.UpdateUser(user);
        }
        public void ChangePassword(int id, string newPass)
        {
            _manager.ChangePassword(id, newPass);
        }

        public void DeleteUser(int id)
        {
            _manager.DeleteUser(id);
        }
    }
}