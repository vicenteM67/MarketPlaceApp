using System;
using System.Linq;
using MarketPlace;

namespace MarketPlace;

class Program
{
    static void Main(string[] args)
    {
        AccountController ctrl = new();
        bool isRunning = true;

        while (isRunning)
        {
            if (ctrl.CurrentUser == null)
            {
                Console.Clear();
                Console.WriteLine("=== Second-Hand Market ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Log In");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                string authChoice = Console.ReadLine() ?? "";

                switch (authChoice)
                {
                    case "1":
                        HandleRegistration(ctrl);
                        break;
                    case "2":
                        HandleLogin(ctrl);
                        break;
                    case "3":
                        isRunning = false;
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Welcome back, {ctrl.CurrentUser.Username}!");
                Console.WriteLine("=== Main Menu ===");
                Console.WriteLine("1. Create Listing");
                Console.WriteLine("2. Browse Listings");
                Console.WriteLine("3. View Sales History");
                Console.WriteLine("4. View My Listings");
                Console.WriteLine("5. View Profile");
                Console.WriteLine("6. Log Out");
                Console.Write("Select an option: ");

                string menuChoice = Console.ReadLine() ?? "";

                switch (menuChoice)
                {
                    case "1":
                        HandleCreateListing(ctrl);
                        break;
                    case "2":
                        HandleBrowse(ctrl);
                        break;
                    case "3":
                        HandleViewHistory(ctrl);
                        break;
                    case "4":
                        HandleViewMyListings(ctrl);
                        break;
                    case "5":
                        ViewProfile(ctrl);
                        break;
                    case "6":
                        ctrl.CurrentUser = null;
                        Console.WriteLine("\nLogged out. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }


    static void HandleCreateListing(AccountController ctrl)
    {
        Console.Clear();
        Console.WriteLine("=== Create New Listing ===");

        Console.Write("Item Name: ");
        string head = Console.ReadLine() ?? "Unnamed Item";

        Console.Write("Description: ");
        string desc = Console.ReadLine() ?? "No description provided.";

        Console.Write("Price: ");
        double.TryParse(Console.ReadLine(), out double price);
        Console.Clear();


        Console.WriteLine("\nSelect Category:");
        Console.WriteLine("1. Electronic");
        Console.WriteLine("2. Pets");
        Console.WriteLine("3. Clothing");
        Console.WriteLine("4. Literature");
        Console.WriteLine("5. Art");
        Console.WriteLine("6. Vehicles");
        Console.WriteLine("7. Sports");
        Console.WriteLine("8. Furniture");
        Console.WriteLine("9. Hobbies");
        Console.WriteLine("10. Other");
        Console.Write("Choose: ");
        
        Category selectedCat; 
        

        int cateIndex = GetValidMenuChoice(1, 10);
        selectedCat = (Category)(cateIndex - 1);

     
        
        Console.Clear();
    
        Console.WriteLine("\nSelect Condition:");
        Console.WriteLine("1. Pristine");
        Console.WriteLine("2. New");
        Console.WriteLine("3. Used");
        Console.WriteLine("4. Good");
        Console.WriteLine("5. Fair");
        Console.WriteLine("6. Unknown");
        Console.Write("Choose: ");
        Condition selectedCond; 
        int condIndex = GetValidMenuChoice(1, 6);
        selectedCond = (Condition)(condIndex - 1);
        

   
        Advert newAd = new Advert(head, desc, price, selectedCat, selectedCond, ctrl.CurrentUser);
        ctrl.AllAdverts.Add(newAd);
        ctrl.CurrentUser.MyAdverts.Add(newAd);
        

        Console.WriteLine("\nListing created successfully!");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    static void HandleBrowse(AccountController ctrl)
    {
        string keyword = ""; 
        bool browsing = true;

        while (browsing)
        {
            Console.Clear();
            Console.WriteLine("=== Market Listings ===");
            if (!string.IsNullOrEmpty(keyword)) 
                Console.WriteLine($"Filtering by: '{keyword}' (Press 'S' to clear or change)");

         
            var available = ctrl.AllAdverts
                .Where(ad => ad.Status == AdvertStatus.Available &&
                             (ad.Title.ToLower().Contains(keyword) || 
                              ad.Description.ToLower().Contains(keyword)))
                .ToList();

            
            if (available.Count == 0)
            {
                Console.WriteLine("\nNo items found.");
            }
            else
            {
                for (int i = 0; i < available.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {available[i].Title,-20} | {available[i].Price,8} kr");
                }
            }

            
            Console.WriteLine("\n[S] Search/Filter | [0] Back to Menu");
            Console.Write("Select item # or Command: ");
            string input = Console.ReadLine()?.ToUpper() ?? "";

            if (input == "0")
            {
                browsing = false; 
            }
            else if (input == "S")
            {
                Console.Write("Enter keyword (or leave blank to show all): ");
                keyword = Console.ReadLine()?.ToLower() ?? "";
            }
            else if (int.TryParse(input, out int index) && index > 0 && index <= available.Count)
            {
                ProcessPurchase(available[index - 1], ctrl);
             
            }
        }
    }

    static void HandleViewHistory(AccountController ctrl)
    {
        Console.Clear();
        Console.WriteLine("=== Your Purchase History ===");
        var myPurchases = ctrl.AllRecords.Where(r => r.Originator == ctrl.CurrentUser).ToList();

        if (myPurchases.Count == 0)
        {
            Console.WriteLine("You haven't bought anything yet.");
        }
        else
        {
            foreach (var rec in myPurchases)
            {
                Console.WriteLine($"- {rec.Item.Title} | {rec.Price} kr | Date: {rec.Timestamp.ToShortDateString()}");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    static void HandleRegistration(AccountController ctrl)
    {
        Console.Write("\nUsername: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        string pass = Console.ReadLine() ?? "";

        if (ctrl.RegisterUser(name, pass)) Console.WriteLine("Registered!");
        else Console.WriteLine("Username taken.");

        Console.ReadKey();
    }

    static void HandleLogin(AccountController ctrl)
    {
        Console.Write("\nUsername: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        string pass = Console.ReadLine() ?? "";

        if (ctrl.Login(name, pass)) Console.WriteLine("Login success!");
        else Console.WriteLine("Failed.");

        Console.ReadKey();
    }

    static void ProcessPurchase(Advert item, AccountController ctrl)
{
    bool inItemView = true;

    while (inItemView)
    {
        Console.Clear();
        Console.WriteLine($"=== {item.Title} ===");
        Console.WriteLine($"Posted on: {item.AdvertTime.ToShortDateString()} at {item.AdvertTime.ToShortTimeString()}");
        Console.WriteLine($"Category: {item.Category} | Condition: {item.Condition}"); 
        Console.WriteLine($"Price: {item.Price} kr | Seller: {item.Seller?.Username}");
        Console.WriteLine($"Description: {item.Description}");
        Console.WriteLine("----------------------------------");

   
        if (item.Seller == ctrl.CurrentUser)
        {
            Console.WriteLine("\n(You are the seller of this item)");
            Console.WriteLine("1. Back");
        }
        else
        {
            Console.WriteLine("\n1. Buy | 2. Back");
        }

        Console.Write("\nChoice: ");
        string choice = Console.ReadLine() ?? "";

        if (choice == "1")
        {
     
            if (item.Seller == ctrl.CurrentUser)
            {
                inItemView = false; 
                break;
            }

            
            item.Status = AdvertStatus.Sold;
            Records rec = new(item, ctrl.CurrentUser, item.Seller, item.Price);
            ctrl.AllRecords.Add(rec);
            ctrl.CurrentUser.PurchaseHistory.Add(rec); 
            item.Seller?.PurchaseHistory.Add(rec);

            Console.WriteLine("\n[SUCCESS] You have purchased this item!");

         
            Console.WriteLine($"\nWould you like to leave a review for {item.Seller?.Username}?");
            Console.Write("Press 1 for YES, any other key to SKIP: ");
            
            if (Console.ReadLine() == "1")
            {
                int rating;
                while (true) 
                {
                    Console.Write("Rating (1-6): ");
                    if (int.TryParse(Console.ReadLine(), out rating) && rating >= 1 && rating <= 6) break;
                    Console.WriteLine("Invalid. Please enter a number between 1 and 6.");
                }

                Console.Write("Comment: ");
                string comment = Console.ReadLine() ?? "";

                Review newReview = new Review(rating, comment, ctrl.CurrentUser);
                item.Seller?.Reviews.Add(newReview);
                Console.WriteLine("Thank you for your review!");
            }

            Console.WriteLine("\nPress any key to return to the Market...");
            Console.ReadKey();
            inItemView = false; 
        }
        else if (choice == "2" || (choice == "1" && item.Seller == ctrl.CurrentUser))
        {
            inItemView = false; 
        }
        else
        {
            Console.WriteLine("Invalid choice. Press any key to try again.");
            Console.ReadKey();
        }
    }
}

    static void HandleViewMyListings(AccountController ctrl)
    {
        
        bool stayInMenu = true;

        while (stayInMenu)
        {
            Console.Clear();
            Console.WriteLine($"=== Listings by {ctrl.CurrentUser.Username} ===");

            var myList = ctrl.AllAdverts.Where(a => a.Seller == ctrl.CurrentUser).ToList();

            if (myList.Count == 0)
            {
                Console.WriteLine("No listings found.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < myList.Count; i++)
                Console.WriteLine($"{i + 1}. {myList[i].Title} | {myList[i].Status}");

            Console.Write("\nSelect to Manage (0 to back): ");
            string input = Console.ReadLine() ?? "";

           
            if (input == "0")
            {
                stayInMenu = false;
            }
            
            else if (int.TryParse(input, out int idx) && idx > 0 && idx <= myList.Count)
            {
                var selected = myList[idx - 1];

                
                Console.WriteLine($"\nManaging: {selected.Title}");
                Console.WriteLine("1. Edit Price | 2. Delete | 3. Cancel");
                string act = Console.ReadLine() ?? "";

                if (act == "1")
                {
                    Console.Write("New Price: ");
                    double.TryParse(Console.ReadLine(), out double p);
                    selected.Price = p;
                    Console.WriteLine("Price Updated!");
                    Console.ReadKey();
                }
                else if (act == "2")
                {
                    ctrl.AllAdverts.Remove(selected);
                    Console.WriteLine("Deleted!");
                    Console.ReadKey();
                }
               
            }
            else
            {
                Console.WriteLine("Invalid input. Please pick a number from the list or 0.");
                Console.ReadKey();
            }
        }
       
    }
    
    static void ViewProfile(AccountController ctrl)
    {
        var user = ctrl.CurrentUser;
        if (user == null) return; 

        Console.Clear();
        Console.WriteLine($"=== Profile: {user.Username} ===");

        
        if (user.Reviews.Any())
        {
            double avg = user.Reviews.Average(r => r.Rating);
            Console.WriteLine($"Average Rating: {avg:F1} / 6");
        }

        Console.WriteLine("\n--- History ---");
       
        var sold = user.PurchaseHistory.Where(r => r.Seller == user);
        var bought = user.PurchaseHistory.Where(r => r.Originator == user);

        foreach (var r in bought) 
            Console.WriteLine($"[Bought] {r.Item.Title} from {r.Seller.Username} on {r.Timestamp:d}");
    
        foreach (var r in sold) 
            Console.WriteLine($"[Sold] {r.Item.Title} to {r.Originator.Username} on {r.Timestamp:d}");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }
    static int GetValidMenuChoice(int min, int max)
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
        {
            Console.Write($"Invalid. Please enter a number between {min} and {max}: ");
        }
        return choice;
    }
}
