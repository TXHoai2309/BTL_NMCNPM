using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserRepository
{
    public List<User> GetAllUsers()
    {
        return HttpContext.Current.Application["dstk"] as List<User>;
    }

    public User GetUser(string username, string password)
    {
        var users = GetAllUsers();

        return users?.FirstOrDefault(u =>
            u.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase)
            && u.Password == password);
    }
    public bool IsUsernameExists(string username)
    {
        var users = GetAllUsers();
        return users?.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? false;
    }

    public void AddUser(User user)
    {
        var users = GetAllUsers();
        users.Add(user);
        HttpContext.Current.Application["dstk"] = users;
    }
}