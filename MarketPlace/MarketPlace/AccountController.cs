namespace MarketPlace;

public class AccountController
{
  
    public List<User> AllUsers { get; set; } = new();
    public List<Advert> AllAdverts { get; set; } = new();
    public List<Records> AllRecords { get; set; } = new();
    public User? CurrentUser { get; set; }

    
    public bool Login(string name, string pass)
    {
        var user = AllUsers.FirstOrDefault(u => u.Username == name && u.Password == pass);
        if (user != null)
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }

    
    public Advert? FindAdvert(string title)
    {
        return AllAdverts.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
    
    public bool RegisterUser(string username, string password)
    {
       
        foreach (var user in AllUsers)
        {
            if (user.Username.ToLower() == username.ToLower())
            {
                return false;
            }
        }

        
        User newUser = new User(username, password);
        AllUsers.Add(newUser);
        return true;
    }
    
}