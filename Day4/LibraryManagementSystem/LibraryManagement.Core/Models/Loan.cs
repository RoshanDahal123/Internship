using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Models;

public class Loan
{
    public int BookId { get; }
    public int BorrowerId { get; }
    public DateTime BorrowedOn { get; }
    public DateTime? DueDate { get; }

    public DateTime? ReturnedOn { get; set; }

    public bool isOverDue => ReturnedOn is null && DateTime.Now > DueDate;

    public Loan(int bookId, int borrowerId, DateTime borrowedOn, DateTime? dueDate, DateTime? returnedOn)
    {
        BookId = bookId;
        BorrowerId = borrowerId;
        BorrowedOn = borrowedOn;
        DueDate = dueDate;
        ReturnedOn = returnedOn;
    }
}