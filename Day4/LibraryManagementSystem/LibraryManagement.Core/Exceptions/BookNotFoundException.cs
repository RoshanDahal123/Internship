using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Exceptions;

    public class BookNotFoundExceptions : Exception
{
    public BookNotFoundExceptions(int bookId):base($"`{bookId}' is not " +
        $"found.Please enter correct bookId") { }
}