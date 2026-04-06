namespace MarketPlace;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    
  
    public List<Advert> MyAdverts { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Records> PurchaseHistory { get; set; } = new();

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    
    
}