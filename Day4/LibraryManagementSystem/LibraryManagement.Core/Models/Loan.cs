using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Models;

public class Loan
{
    public int BookId { get; }
    public int BorrowerId { get; }
    public DateTime BorrowedOn { get; }
    public DateTime DueDate { get; }

    public DateTime? ReturnedOn { get; set; }

    public bool IsOverDue => ReturnedOn is null && DateTime.Now > DueDate;

    public Loan(int bookId, int borrowerId, DateTime borrowedOn,int loanDays=14)
    {
        BookId = bookId;
        BorrowerId = borrowerId;
        BorrowedOn = borrowedOn;
        DueDate = borrowedOn.AddDays(loanDays);
       
    }
}