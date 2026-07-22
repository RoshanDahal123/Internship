using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Exceptions;

// Custom exceptions make error handling upstream much clearer than generic Exception.

public class BookNotAvailableException: Exception
{
    public BookNotAvailableException(string bookTitle) : base($"`{bookTitle}' is not " +
        $"available for borrowing right now.")
    { }

}

