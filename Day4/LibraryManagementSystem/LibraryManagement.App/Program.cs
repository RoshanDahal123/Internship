using LibraryManagement.Core.Models;
using LibraryManagement.Data.Repositories;
using LibraryManagement.Services;

var bookRepo = new Repository<Book>(b => b.Id, "books.json");
var loanRepo = new Repository<Loan>(l => l.BookId, "loans.json");


await bookRepo.LoadFromFileAsync();
await loanRepo.LoadFromFileAsync();

var libraryService = new LibraryService(bookRepo, loanRepo);
new NotificationService().Subscribe(libraryService);

var student = new Student(1, "Aarav Shrestha", "aarav@example.com", "R-101");
bookRepo.Add(new Book(1, "Clean Code", "Robert C. Martin", "9780132350884"));
bookRepo.Add(new Book(2, "The Pragmatic Programmer", "Hunt & Thomas", "9780201616224"));

bool running = true;

while (running)
{
    Console.WriteLine("\n1) Borrow  2) Return  3) Search  4) Overdue  5) Save & exit");
    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Console.Write("Book id: ");
                int bId = int.Parse(Console.ReadLine()!);
                libraryService.BorrowBook(bId, student, student.Id);
                break;
            case "2":
                Console.Write("Book id: ");
                int rId = int.Parse(Console.ReadLine()!);
                libraryService.ReturnBook(rId, student);
                break;
            case "3":
                Console.Write("Keyword: ");
                foreach (var b in libraryService.SearchByTitle(Console.ReadLine() ?? ""))
                    Console.WriteLine(b.Title);
                break;
            case "4":
                foreach (var loan in libraryService.GetOverdueLoans())
                    Console.WriteLine($"Overdue: book {loan.BookId}, due {loan.DueDate:d}");
                break;
            case "5":
                await bookRepo.SaveToFileAsync();
                await loanRepo.SaveToFileAsync();
                running = false;
                break;
            default:
                Console.WriteLine("Unknown option.");
                break;
        }
    }
    catch (Exception ex)
    {
        // Every domain exception (BookNotAvailableException, MemberNotFoundException, etc.)
        // and any unexpected one lands here so the app never crashes mid-session.
        Console.WriteLine($"Error: {ex.Message}");
    }
}
