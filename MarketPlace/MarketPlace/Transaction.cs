namespace MarketPlace;


public abstract class Transaction
{
    
    public DateTime Timestamp { get; private set; }
    public User Originator { get; private set; }

    protected Transaction(User originator)
    {
        Originator = originator;
        Timestamp = DateTime.Now; 
    }
}


public class Records : Transaction
{
    public Advert Item { get; private set; }
    public User Seller { get; private set; }
    public double Price { get; private set; }

    public Records(Advert item, User buyer, User seller, double price) : base(buyer)
    {
        Item = item;
        Seller = seller;
        Price = price;
    }
}


public class Review : Transaction
{
    public int Rating { get; private set; }
    public string Comment { get; private set; }

    public Review(int rating, string comment, User reviewer) : base(reviewer)
    {
        
        Rating = Math.Clamp(rating, 1, 6);
        Comment = comment;
    }
}