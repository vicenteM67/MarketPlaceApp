namespace MarketPlace;

public class Advert
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    
    public Category Category { get; set; } 
    public Condition Condition { get; set; } 
    public User Seller { get; set; }
    public AdvertStatus Status { get; set; }
    public DateTime AdvertTime { get; set; }
    
    

    public Advert(string title, string description, double price, Category category, Condition condition, User seller)
    {
       
    
        this.Title = title;
        this.Description = description;
        this.Price = price;
        this.Category = category; 
        this.Condition = condition;  
        this.Seller = seller;
        this.Status = AdvertStatus.Available;
        this.AdvertTime = DateTime.Now;
       
    }
}    