using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Models;

public class Loan
{
    public int Id { get; }
    public int BookId { get; }
    public int BorrowerId { get; }
    public DateTime BorrowedOn { get; }
    public DateTime DueDate { get; }

    public DateTime? ReturnedOn { get; set; }

    public bool IsOverDue => ReturnedOn is null && DateTime.Now > DueDate;

    public Loan(int id,int bookId, int borrowerId, DateTime borrowedOn, DateTime dueDate, DateTime? returnedOn)
    {
        Id = id;
        BookId = bookId;
        BorrowerId = borrowerId;
        BorrowedOn = borrowedOn;
        DueDate = dueDate;
        ReturnedOn = returnedOn;

    }

    // Keeps the "loan period = 14 days" rule in exactly one place.
    public static Loan Create(int id, int bookId, int borrowerId, DateTime borrowedOn, int loanDays = 0) =>
        new(id, bookId, borrowerId, borrowedOn, borrowedOn.AddDays(loanDays), returnedOn: null);
}