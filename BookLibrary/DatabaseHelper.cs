using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace BookLibrary;

public class DatabaseHelper : IDisposable
{
    private string _connectionString;
    private SqliteConnection _connection;

    public DatabaseHelper()
    {
        _connectionString = "Data Source=LibraryDatabase.db";
        _connection = new SqliteConnection(_connectionString);
    }

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new SqliteConnection(_connectionString);
    }

    public void OpenConnection()
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (_connection != null && _connection.State != ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

    public void Dispose()
    {
        CloseConnection();
        _connection?.Dispose();
    }

    public void InitializeDatabase()
    {
        OpenConnection();

        string createTableQuery = @"CREATE TABLE Books(
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Title TEXT NOT NULL,
                                Author TEXT NOT NULL,
                                Genre TEXT NOT NULL,
                                PublicationYear INTEGER
                                );";

        using (var command = new SqliteCommand(createTableQuery, _connection))
        {
            command.ExecuteNonQuery();
        }

        CloseConnection();
    }

    public void CreateBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        {
            throw new ArgumentException("Title and Author cannot be empty.");
        }

        try
        {
            OpenConnection();

            string insertQuery = @"INSERT INTO Books(Title, Author, Genre, PublicationYear) 
                                VALUES(@Title, @Author, @Genre, @PublicationYear);";

            using (var command = new SqliteCommand(insertQuery, _connection))
            {
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Genre", book.Genre);
                command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);

                command.ExecuteNonQuery();
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine("There was an error saving the book. Please try again later.");
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
    }

    public List<Book> GetBooks()
    {
        var books = new List<Book>();

        try
        {
            OpenConnection();
            string selectQuery = "SELECT * FROM Books";

            using (var command = new SqliteCommand(selectQuery, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var book = new Book(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Title")),
                            reader.GetString(reader.GetOrdinal("Author")),
                            reader.GetString(reader.GetOrdinal("Genre")),
                            reader.GetInt32(reader.GetOrdinal("PublicationYear"))
                        );
                        books.Add(book);
                    }
                }
            }
        }
        catch (SqliteException ex)
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Unknown problem: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }

        return books;
    }

    public Book FindBookById(int id)
    {
        Book book = null;

        try
        {
            OpenConnection();
            string findIdQuery = "SELECT * FROM Books WHERE Id=@Id";

            using (var command = new SqliteCommand(findIdQuery, _connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        book = new Book(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Title")),
                            reader.GetString(reader.GetOrdinal("Author")),
                            reader.GetString(reader.GetOrdinal("Genre")),
                            reader.GetInt32(reader.GetOrdinal("PublicationYear"))
                        );
                    }
                }
            }
        }
        catch (SqliteException ex)
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }

        return book;
    }

    public void UpdateBook(Book book)
    {
        try
        {
            OpenConnection();
            string updateQuery = @"UPDATE Books SET Title = @Title, 
                Author = @Author, Genre = @Genre, PublicationYear = @PublicationYear WHERE Id = @Id;";

            using (var command = new SqliteCommand(updateQuery, _connection))
            {
                command.Parameters.AddWithValue("@Id", book.Id);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Genre", book.Genre ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);

                int rows = command.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new Exception("Update failed. Book not found.");
                }
            }
        }
        catch (SqliteException ex)
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
    }

    public void DeleteBook(int Id)
    {
        try
        {
            OpenConnection();
            string deleteQuery = "DELETE FROM Books WHERE Id = @Id;";

            using (var command = new SqliteCommand(deleteQuery, _connection))
            {
                command.Parameters.AddWithValue("@Id", Id);

                int rows = command.ExecuteNonQuery();

                if (rows == 0)
                {
                    System.Console.WriteLine("No book with given ID.");
                }
            }
        }
        catch (SqliteException ex)
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error while deleting book: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }
    }

    public List<Book> SearchBooksByCriteria(string criteria, string searchTerm)
    {
        var books = new List<Book>();

        try
        {
            OpenConnection();
            string searchQuery = $"SELECT * FROM Books WHERE {criteria} LIKE @searchTerm";

            using (var command = new SqliteCommand(searchQuery, _connection))
            {
                command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var book = new Book(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Title")),
                            reader.GetString(reader.GetOrdinal("Author")),
                            reader.IsDBNull(reader.GetOrdinal("Genre")) ? null : reader.GetString(reader.GetOrdinal("Genre")),
                            reader.GetInt32(reader.GetOrdinal("PublicationYear"))
                        );
                        books.Add(book);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error during search: {ex.Message}");
        }
        finally
        {
            CloseConnection();
        }

        return books;
    }
}
