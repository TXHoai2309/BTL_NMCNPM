using System;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class UserManager
    {
        private UserRepository repo = new UserRepository();

        public User Login(string username, string password)
        {
            return repo.Login(username, password);
        }

        public bool Register(User u)
        {
            if (repo.IsUsernameExists(u.Username))
                return false;

            repo.Register(u);
            return true;
        }

        public User GetUserByID(int id)
        {
            return repo.GetUserByID(id);
        }

        public void UpdateUser(User u)
        {
            repo.UpdateUser(u);
        }
        public bool IsUsernameExists(string username)
        {
            return repo.IsUsernameExists(username);
        }
        public void ChangePassword(int id, string newPass)
        {
            repo.ChangePassword(id, newPass);
        }

        public void DeleteUser(int id)
        {
            repo.DeleteUser(id);
        }
    }
}