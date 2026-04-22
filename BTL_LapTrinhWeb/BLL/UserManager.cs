using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class UserManager
    {
        private UserRepository _repo = new UserRepository();

        // ================= LOGIN =================
        public User ValidateUser(string username, string password)
        {
            var users = _repo.GetAllUsers();

            return users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password
            );
        }

        // ================= CHECK USERNAME =================
        public bool IsUsernameExists(string username)
        {
            var users = _repo.GetAllUsers();

            return users.Any(u => u.Username == username);
        }

        // ================= REGISTER =================
        public void Register(User user)
        {
            var users = _repo.GetAllUsers();

            // tự tăng ID (nếu chưa có DB)
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

            users.Add(user);

            _repo.SaveUsers(users);
        }

        // ================= GET USER =================
        public User GetUserById(int id)
        {
            return _repo.GetAllUsers()
                        .FirstOrDefault(u => u.Id == id);
        }

        // ================= UPDATE =================
        public void UpdateUser(User updated)
        {
            var users = _repo.GetAllUsers();

            var u = users.FirstOrDefault(x => x.Id == updated.Id);
            if (u == null) return;

            u.Address1 = updated.Address1;
            u.Address2 = updated.Address2;
            u.Postal = updated.Postal;
            u.City = updated.City;
            u.Country = updated.Country;

            _repo.SaveUsers(users);
        }

        // ================= GET ORDER =================
        public List<Order> GetUserOrders(string username)
        {
            var orders = _repo.GetAllOrders();

            return orders
                .Where(o => o.UserName == username)
                .OrderByDescending(o => o.Date)
                .ToList();
        }
    }
}