using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Models;

public class Book
{
    public int Id { get;}
    public string Title { get; set; }

    public string Author { get; set; }
    public string Isbn { get; set; }

    public Book(int id, string title, string author , string isbn)
    {
    Id=id;
        Title = title;
    Author= author;
    Isbn=isbn;
    }
}