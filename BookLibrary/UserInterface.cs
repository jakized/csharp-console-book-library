using System;

namespace BookLibrary;

public class UserInterface
{
    private readonly LibraryController _libraryController;

    public UserInterface(LibraryController libraryController)
    {
        _libraryController = libraryController;
    }


    public void MainMenu()
    {
        bool running = true;

        while (running)
        {
            // Display the menu options
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("       Book Library Manager      ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. List All Books");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Delete Book");
            Console.WriteLine("5. Filter Books");
            Console.WriteLine("6. Exit");
            Console.WriteLine("=================================");
            Console.Write("Choose an option: ");

            // Get user input
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddBookMenu();
                    break;
                case "2":
                    ViewAllBooksMenu();
                    break;
                case "3":
                    UpdateBookMenu();
                    break;
                case "4":
                    DeleteBookMenu();
                    break;
                case "5":
                    SearchBookMenu();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Exiting the application...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public void AddBookMenu()
    {
        Console.Clear();

        System.Console.Write("Title: ");
        string title = Console.ReadLine();

        System.Console.Write("Author: ");
        string author = Console.ReadLine();

        System.Console.Write("Genre: ");
        string genre = Console.ReadLine();

        System.Console.Write("Publication year: ");
        if (int.TryParse(Console.ReadLine(), out var year))
        {
            try
            {
                _libraryController.AddBook(title, author, genre, year);
                System.Console.WriteLine("Book added successfully!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error creating book: {ex.Message}");
            }
        }
        else
        {
            System.Console.WriteLine("Invalid publication year. Try again.");
        }

        System.Console.WriteLine("Press any key to return to main menu...");
        Console.ReadKey();
    }

    public void ViewAllBooksMenu()
    {
        Console.Clear();
        Console.WriteLine("Loading books, please wait...");

        var spinner = new[] { '|', '/', '-', '\\' };
        int count = 0;
        DateTime endTime = DateTime.Now.AddSeconds(3);

        while (DateTime.Now < endTime)
        {
            Console.SetCursorPosition(0, 1);
            System.Console.Write(spinner[count % spinner.Length]);
            count++;
            Thread.Sleep(100);
        }

        Console.SetCursorPosition(0, 1);
        Console.Write(new string(' ', 1));


        var books = _libraryController.ListAllBooks();

        if (books.Any())
        {
            System.Console.WriteLine("Books in the library: ");
            System.Console.WriteLine(new string('-', 102));
            System.Console.WriteLine($"| {"Id",-5} | {"Title",-30} | {"Author",-20} | {"Genre",-15} | {"Publication Year",4} |");
            System.Console.WriteLine(new string('-', 102));
            foreach (var book in books)
            {
                System.Console.WriteLine(book);
            }
            System.Console.WriteLine(new string('-', 102));
        }
        else
        {
            System.Console.WriteLine("No books found in the library.");
        }

        System.Console.WriteLine("Press any key to return to continue...");
        System.Console.ReadKey();
    }

    public void UpdateBookMenu()
    {
        Console.Clear();

        List<Book> books = _libraryController.ListAllBooks();
        if (books.Count == 0)
        {
            Console.WriteLine("No books available. Press any key to return to the main menu...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("List of Books:");
        foreach (var el in books)
        {
            Console.WriteLine(el);
        }
        Console.WriteLine();

        System.Console.Write("Enter the ID of the Book you wanna change: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            System.Console.WriteLine("Enter a valid number. Press any key to go back and try again...");
            Console.ReadKey();
            return;
        }

        var book = _libraryController.GetBookById(id);
        if (book == null)
        {
            System.Console.WriteLine("No Book with that ID. Press any key to go back and try again...");
            System.Console.ReadKey();
            return;
        }

        Console.Clear();
        System.Console.WriteLine("Current Book: ");
        System.Console.WriteLine(book);
        System.Console.WriteLine();

        System.Console.Write("Enter the new title (Leave blank to keep current): ");
        string title = Console.ReadLine();

        System.Console.Write("Enter a new Author (Leave blank to keep current): ");
        string author = Console.ReadLine();

        System.Console.Write("Enter a new Genre (Leave blank to keep current): ");
        string genre = Console.ReadLine();

        System.Console.Write("Enter a new Publication Year (Leave blank to keep current): ");
        string inputYear = Console.ReadLine();
        int year;
        while (!int.TryParse(inputYear, out year) || (year < 0 || year > DateTime.Now.Year))
        {
            System.Console.Write("Enter a valid year: ");
            inputYear = Console.ReadLine();
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            book.Title = title;
        }
        if (!string.IsNullOrWhiteSpace(author))
        {
            book.Author = author;
        }
        if (!string.IsNullOrWhiteSpace(genre))
        {
            book.Genre = genre;
        }
        book.PublicationYear = year;

        _libraryController.UpdateBook(book);

        Console.WriteLine("Book updated successfully. Press any key to return to the main menu...");
        Console.ReadKey();
    }

    public void DeleteBookMenu()
    {
        System.Console.WriteLine("List of Books: ");
        ViewAllBooksMenu();

        System.Console.Write("Enter the ID of Book you wish to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _libraryController.DeleteBook(id);
        }
        else
        {
            System.Console.WriteLine("Invalid ID format, enter a valid number.");
        }

        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }

    public void SearchBookMenu()
    {
        Console.Clear();
        Console.WriteLine("Search Books By:");
        Console.WriteLine("1. Title");
        Console.WriteLine("2. Author");
        Console.WriteLine("3. Genre");
        Console.WriteLine("4. Publication Year");
        Console.Write("Choose an option: ");

        string option = Console.ReadLine();
        string searchTerm = "";

        switch (option)
        {
            case "1":
                Console.Write("Enter title: ");
                searchTerm = Console.ReadLine();
                _libraryController.SearchBooks("Title", searchTerm);
                break;
            case "2":
                Console.Write("Enter author: ");
                searchTerm = Console.ReadLine();
                _libraryController.SearchBooks("Author", searchTerm);
                break;
            case "3":
                Console.Write("Enter genre: ");
                searchTerm = Console.ReadLine();
                _libraryController.SearchBooks("Genre", searchTerm);
                break;
            case "4":
                Console.Write("Enter publication year: ");
                searchTerm = Console.ReadLine();
                _libraryController.SearchBooks("PublicationYear", searchTerm);
                break;
            default:
                Console.WriteLine("Invalid option, returning to the main menu.");
                break;
        }
    }
}
