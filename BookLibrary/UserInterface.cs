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
            Console.WriteLine("Welcome to the Book Library!");
            Console.WriteLine("1. Add a Book");
            Console.WriteLine("2. View All Books");
            System.Console.WriteLine("3. Update Book Information");
            Console.WriteLine("3. Exit");
            Console.Write("Please choose an option: ");

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

        var spinner = new[] {'|', '/', '-', '\\'};
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

        System.Console.WriteLine("Press any key to return to main menu...");
        System.Console.ReadKey();
    }
}
