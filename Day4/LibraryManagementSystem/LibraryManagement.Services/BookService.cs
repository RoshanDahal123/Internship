

using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Data.Repositories;

namespace LibraryManagement.Services;
using LibraryManagement.Core.Enums;

public class BookService
{

    private readonly Repository<Book> _books;
    public BookService(Repository<Book> books)
    {
        _books = books;
    }

    public Book AddBook(string title, string author ,string isbn)
    {
        if(_books.GetAll().Any(b=>b.Isbn == isbn))
        {
            throw new DuplicateEntityException($"A book with ISBN {isbn} already exists.");
            
        }
        var book = new Book(_books.GetNextId(), title, author, isbn);
        _books.Add(book);
        return book;
    }

    public void UpdateBook(int id, string title, string author, string isbn)
    {
        var book = _books.GetById(id) ?? throw new MemberNotFoundException(id);
        book.Title = title;
        book.Author = author;
        book.Isbn = isbn;
        _books.Update(book);
    }

    public void DeleteBook(int id)
    {
        var book = _books.GetById(id) ?? throw new MemberNotFoundException(id);
        if (book.Status == BooksStatus.Borrowed)
            throw new InvalidOperationException("Cannot delete a book that is currently borrowed.");
        _books.Remove(id);
    }

    public Book? GetBook(int id) => _books.GetById(id);

    public IEnumerable<Book> GetAllBooks() => _books.GetAll();

    public IEnumerable<Book> Search(string keyword)=>
        _books.GetAll().Where(b=>
          b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Isbn.Contains(keyword, StringComparison.OrdinalIgnoreCase));
}