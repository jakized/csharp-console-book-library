📚 Book Library Manager (C# Console Application)
This is a Book Library Management console application built with C# and SQLite. It allows users to Add, Update, Delete, List, and Search books in a local database.

🚀 Features
CRUD Operations: Create, Read, Update, Delete books.
SQLite Database Integration: Persistent local database for storing book details.
Searching: Option to search for books by title, author, genre, and publication year.

🛠️ Project Structure
├── BookLibraryManager
│   ├── Book.cs               # book entity.
│   ├── LibraryController.cs   # Business logic and operations on books.
│   ├── DatabaseHelper.cs      # SQLite database connection and queries.
│   ├── UserInterface.cs       # Interaction with the user and displays UI.
│   └── Program.cs             # Application entry point, showing the main menu.


Classes and Responsibilities

## Book.cs
Represents a book with properties like Id, Title, Author, Genre, PublicationYear.
Overridden ToString() for formatted display of book details.


##LibraryController.cs
Contains methods for managing books:
AddBook()
UpdateBook()
DeleteBook()
FilterBooks()

## DatabaseHelper.cs
Manages the database connection and queries:
OpenConnection()
CreateBook()
GetBooks()
UpdateBookById()
DeleteBookById()

## UserInterface.cs
Displays the Main Menu and handles user input.
Provides feedback, displays books, and interacts with the controller.


🖥️ Usage
Clone the repository.

Build and run the application:
dotnet build
dotnet run


Navigate the menu to manage your book library:

Option 1: Add a new book.
Option 2: List all books.
Option 3: Update an existing book.
Option 4: Delete a book by ID.
Option 5: Filter books by various fields.


📦 Dependencies
.NET Core SDK
SQLite (Integrated via Microsoft.Data.Sqlite)
