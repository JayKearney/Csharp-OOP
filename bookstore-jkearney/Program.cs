using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;

public enum Genre { Fiction, Drama, Science }

public class Book
{
    private string title;
    private string author;
    private Genre genre;
    private double price;
    private int copiesAvailable;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Author
    {
        get { return author; }
        set { author = value; }
    }

    public Genre Genre
    {
        get { return genre; }
        set { genre = value; }
    }

    public double Price
    {
        get { return price; }
        set { if (value >= 1) price = value; else throw new ArgumentException("Invalid Price."); }
    }


    public int CopiesAvailable
    {
        get { return copiesAvailable; }
        //d handling runtime errors
        set { if (value >= 1) copiesAvailable = value; else throw new ArgumentException("Invalid number of copies."); }
    }
    public bool Purchase(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Invalid purchase Quantity.");
        }

        if (quantity > copiesAvailable)
        {
            throw new ArgumentException("The Quantity cannot exceeds the Copies available.");
        }

        copiesAvailable -= quantity; // Reduce the available copies
        return true; 
        
    }


}

public abstract class Person
{
    //b use of private access modifier for encapsulation
    private string name;
    private string email;


    protected Person(string name, string email)
    {
        //c. use of this keyword
        this.name = name;
        this.email = email;
    }
}
//b Inheritance
public class Customer : Person
{
    public Customer(string name, string email) : base(name, email) { }
}

public class Staff : Person
{
    //c static class member 
    public static double StaffDiscount { get; } = 0.05;

    public Staff(string name, string email) : base(name, email) { }

    public static double CalculateDiscountedPrice(double price)
{
    return price - (price * StaffDiscount);
}

}

public class Bookstore
{
    private List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    // Method to process a purchase of a book
    public bool PurchaseBook(string title, int quantity, Person buyer)
    {
        var book = books.FirstOrDefault(
            b => 
        b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return false;
        }

        if (!book.Purchase(quantity))
        {
            return false; 
        }

        if (buyer is Staff)
        {
            // Discount applied
            Console.WriteLine($"Purchase successful. Staff discount applied. Final price: {Staff.CalculateDiscountedPrice(book.Price * quantity)}");
        }
        else
        {
            Console.WriteLine($"Purchase successful. Final price: {book.Price * quantity}");
        }
        return true;
           
    }

}

class Program
{
    static void Main(string[] args)
    {
        // Create the bookstore
        var bookstore = new Bookstore();
        var b1 = new Book()
        {
            Title = "The Great Fish",
            Author = "William Wordsworth",
            Genre = Genre.Fiction,
            Price = 50,
            CopiesAvailable = 5
        };

        // Create more books
        var b2 = new Book()
    {
    Title = "Romeo and Juliet",
    Author = "Shakespeare",
    Genre = Genre.Drama,
    Price = 40,
    CopiesAvailable = 10
    };

    var b3 = new Book()
    {
    Title = "The Code Breaker",
    Author = "Walter Isaacson",
    Genre = Genre.Science,
    Price = 30,
    CopiesAvailable = 8
    };

        
    // Add some books to the bookstore
    bookstore.AddBook(b1);
    bookstore.AddBook(b2);
    bookstore.AddBook(b3);

    // Create a customer and a staff member
    var customer = new Customer("Jane Doe", "jane.doe@example.com");
    var staff = new Staff("John Smith", "john.smith@example.com");

        // Get user input for book details
        Console.Write("Enter the book title: ");
        string bookTitle = Console.ReadLine();

        int quantity;
        do
        {
            Console.Write("Enter the quantity (positive integer): ");
        } while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0);

        string role;
        do
        {
            Console.Write("Enter the role (Customer/Staff): ");
            role = Console.ReadLine();
        } while (!role.Equals("Customer", StringComparison.OrdinalIgnoreCase) && !role.Equals("Staff", StringComparison.OrdinalIgnoreCase));

        // Attempt to purchase a book based on user input
        if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"\n{role} purchasing '{bookTitle}' x{quantity}:");
            bookstore.PurchaseBook(bookTitle, quantity, customer);
        }
        else if (role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"\n{role} purchasing '{bookTitle}' x{quantity}:");
            bookstore.PurchaseBook(bookTitle, quantity, staff);
        }

        Console.ReadLine(); // Added to keep the console open for demonstration
    }
}

