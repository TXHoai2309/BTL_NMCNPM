using BTL_LapTrinhWeb.Models;

public class UserManager
{
    private UserRepository repo = new UserRepository();

    public User ValidateUser(string username, string password)
    {
        return repo.GetUser(username, password);
    }
    public bool IsUsernameExists(string username)
    {
        return repo.IsUsernameExists(username);
    }

    public void Register(User user)
    {
        repo.AddUser(user);
    }
}