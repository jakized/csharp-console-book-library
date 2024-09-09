using System;

namespace BookLibrary;

public class LibraryController
{
    private readonly DatabaseHelper _databaseHelper;

    public LibraryController(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public void AddBook(string title, string author, string genre, int publicationYear)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Title and author cannot be empty.");
        }

        Book book = new Book(0, title, author, genre, publicationYear);

        _databaseHelper.CreateBook(book);
    }

    public List<Book> ListAllBooks()
    {
        try
        {
            var books = _databaseHelper.GetBooks();
            return books;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error retrieving books. {ex.Message}");
            return new List<Book>();
        }
    }

    public Book GetBookById(int id)
    {
        return _databaseHelper.FindBookById(id);
    }

    public void UpdateBook(Book book)
    {
        _databaseHelper.UpdateBook(book);
    }

    public void DeleteBook(int id)
    {
        var book = _databaseHelper.FindBookById(id);

        if (book == null)
        {
            System.Console.WriteLine("No book found with the given ID.");
            return;
        }

        try
        {
            _databaseHelper.DeleteBook(id);
            System.Console.WriteLine($"Book '{book.Title}' deleted.");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error while deleting book: {ex.Message}");
        }
    }

    public void SearchBooks(string searchCriteria, string searchTerm)
    {
        var foundBooks = _databaseHelper.SearchBooksByCriteria(searchCriteria, searchTerm);

        if (foundBooks.Count == 0)
        {
            System.Console.WriteLine("No books match your criteria.");
        }
        else
        {
            System.Console.WriteLine("Search results:\n");
            foreach (var book in foundBooks)
            {
                System.Console.WriteLine(book.ToString());
                
            }
        }
        System.Console.WriteLine("\nPress any key to return to previous menu: ");
        Console.ReadKey();
    }
}
