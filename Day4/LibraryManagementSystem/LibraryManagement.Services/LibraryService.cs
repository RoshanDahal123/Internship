using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;
using LibraryManagement.Data.Repositories;
using LibraryManagement.Core.Enums;
namespace LibraryManagement.Services;


public class LibraryService
{
    private readonly Repository<Book> _books;
    private readonly Repository<Loan> _loans;
    // Delegate + event: LibraryService announces things happened,
    // it doesn't care who's listening (could be a console logger, an email service, both).

    public event Action<Book>? BookBorrowed;
    public event Action<Book>? BookReturned;

    public LibraryService(Repository<Book> books, Repository<Loan> loans)
    {
        _books = books;
        _loans = loans;
    }

    public void BorrowBook(int bookId, IBorrower borrower, int borrowerId)

    {
        var book = _books.GetById(bookId)
        ?? throw new BookNotFoundExceptions(bookId);


        if (book.Status!= BooksStatus.Available)
            throw new BookNotAvailableException(book.Title);

        if (borrower.BorrowedBooksIds.Count >= borrower.MaxBooksAllowed)
            throw new InvalidOperationException("Borrowing limit reached");
        book.Status = BooksStatus.Borrowed;
        borrower.BorrowedBooksIds.Add(bookId);
        _loans.Add(new Loan(bookId, borrowerId, DateTime.Now));
        BookBorrowed?.Invoke(book);//raise the event

    }

    public void ReturnBook(int bookId, IBorrower borrower)
    {
       var book = _books.GetById(bookId) ?? throw new MemberNotFoundException(bookId);
       var loan = _loans.GetAll().FirstOrDefault(loan=>loan.BookId==bookId && loan.ReturnedOn is null);

     book.Status= BooksStatus.Available;
        borrower.BorrowedBooksIds.Remove(bookId);
        if (loan is not null) loan.ReturnedOn = DateTime.Now;

        BookReturned?.Invoke(book);
    }


    public IEnumerable<Loan> GetOverdueLoans() =>
      _loans.GetAll().Where(l => l.IsOverDue);

    public IEnumerable<Book> SearchByTitle(string keyword) =>
        _books.GetAll()
        .Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        .OrderBy(b => b.Title);

    public Dictionary<BooksStatus, int> GetInventorySummary() =>
        _books.GetAll()
            .GroupBy(b => b.Status)
            .ToDictionary(g => g.Key, g => g.Count());

}

