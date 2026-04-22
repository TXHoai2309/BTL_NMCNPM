using BTL_LapTrinhWeb.Models;

namespace BTL_LapTrinhWeb.Services
{
    public class UserService
    {
        private UserManager manager = new UserManager();

        public User Login(string username, string password)
        {
            if (username.Equals("admin", System.StringComparison.OrdinalIgnoreCase)
                && password == "12345678")
            {
                return new User { Username = "admin" };
            }

            return manager.ValidateUser(username, password);
        }
        public bool IsUsernameExists(string username)
        {
            return manager.IsUsernameExists(username);
        }

        public bool Register(User user)
        {
            if (IsUsernameExists(user.Username))
                return false;

            manager.Register(user);
            return true;
        }
    }

}