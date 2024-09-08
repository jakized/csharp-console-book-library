using System;
using System.Data.Common;

namespace BookLibrary;

public class Book
{
    
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public int PublicationYear { get; set; }


    public Book(int id, string title, string author, string genre, int publicationYear)
    {
        if (publicationYear > DateTime.Now.Year || publicationYear < 0)
        {
            throw new ArgumentException("Invalid publication year.");
        }
        Id = id;
        Title = title;
        Author = author;
        Genre = genre;
        PublicationYear = publicationYear;
    }

    public override string ToString()
    {
        return $"| {Id,-5} | {Title,-30} | {Author,-20} | {Genre,-15} | {PublicationYear,16} |";
    }
}
