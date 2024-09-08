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
}
