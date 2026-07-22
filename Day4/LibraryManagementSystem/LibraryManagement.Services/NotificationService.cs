using System;
using System.Collections.Generic;
using System.Text;
using LibraryManagement.Core.Models;
 namespace LibraryManagement.Services;


public class NotificationService
{
    public void Subscribe(LibraryService service)
    {
        service.BookBorrowed+= book => Console.WriteLine($"[notify] '{book.Title}' checked out.");
        service.BookReturned+= book => Console.WriteLine($"[notify] '{book.Title}' returned. Thank you!");
    }
}
