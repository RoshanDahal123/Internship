using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Data.Persistence;
using LibraryManagement.Data.Repositories;


using LibraryManagement.Services;
var bookRepo = new Repository<Book>(b => b.Id, new JsonFileStore<Book>("books.json"));
var studentRepo = new Repository<Student>(s => s.Id, new JsonFileStore<Student>("students.json"));
var librarianRepo = new Repository<Librarian>(l => l.Id, new JsonFileStore<Librarian>("librarians.json"));
var loanRepo = new Repository<Loan>(l => l.Id, new JsonFileStore<Loan>("loans.json"));

await bookRepo.LoadFromFileAsync();
await studentRepo.LoadFromFileAsync();
await librarianRepo.LoadFromFileAsync();
await loanRepo.LoadFromFileAsync();


var bookService = new BookService(bookRepo);
var studentService = new StudentService(studentRepo);
var librarianService = new LibrarianService(librarianRepo);
var libraryService = new LibraryService(bookRepo, loanRepo);


libraryService.BookBorrowed += b => Console.WriteLine($"[notify] '{b.Title}' checked out.");
libraryService.BookReturned += b => Console.WriteLine($"[notify] '{b.Title}' returned. Thank you!");

bool running = true;

while (running)
{

    Console.WriteLine("""

        === Library Management System ===
        1) Book menu
        2) Student menu
        3) Librarian menu
        4) Borrow book
        5) Return book
        6) Overdue loans
        7) Save & exit
        """);
    Console.Write("Choose: ");


    try
    {
        switch (Console.ReadLine())
        {
            case "1": BookMenu(); break;
            case "2": StudentMenu(); break;
            case "3": LibrarianMenu(); break;
            case "4": BorrowFlow(); break;
            case "5": ReturnFlow(); break;
            case "6": ShowOverdue(); break;
            case "7":
                await bookRepo.SaveToFileAsync();
                await studentRepo.SaveToFileAsync();
                await librarianRepo.SaveToFileAsync();
                await loanRepo.SaveToFileAsync();
                running = false;
                break;
            default:
                Console.WriteLine("Unknown option.");
                break;
        }
    }
    
    
    catch (DuplicateEntityException ex) { Console.WriteLine($"Duplicate: {ex.Message}"); }
    catch (MemberNotFoundException ex) { Console.WriteLine($"Not found: {ex.Message}"); }
    catch (BookNotAvailableException ex) { Console.WriteLine($"Unavailable: {ex.Message}"); }
    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
}
// ---------- Book menu ----------
void BookMenu()
{
    Console.WriteLine("1) Add  2) Edit  3) Delete  4) Find  5) List all");
    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Title: "); var t = Console.ReadLine()!;
            Console.Write("Author: "); var a = Console.ReadLine()!;
            Console.Write("ISBN: "); var isbn = Console.ReadLine()!;
            var book = bookService.AddBook(t, a, isbn);
            Console.WriteLine($"Added book #{book.Id}.");
            break;

        case "2":
            Console.Write("Book id: "); var editId = int.Parse(Console.ReadLine()!);
            Console.Write("New title: "); var nt = Console.ReadLine()!;
            Console.Write("New author: "); var na = Console.ReadLine()!;
            Console.Write("New ISBN: "); var ni = Console.ReadLine()!;
            bookService.UpdateBook(editId, nt, na, ni);
            Console.WriteLine("Updated.");
            break;

        case "3":
            Console.Write("Book id: "); bookService.DeleteBook(int.Parse(Console.ReadLine()!));
            Console.WriteLine("Deleted.");
            break;

        case "4":
            Console.Write("Keyword: ");
            foreach (var b in bookService.Search(Console.ReadLine() ?? ""))
                Console.WriteLine($"[{b.Id}] {b.Title} — {b.Author} ({b.Status})");
            break;

        case "5":
            foreach (var b in bookService.GetAllBooks())
                Console.WriteLine($"[{b.Id}] {b.Title} — {b.Author} ({b.Status})");
            break;
    }
}

// ---------- Student menu ----------
void StudentMenu()
{
    Console.WriteLine("1) Add  2) Edit  3) Delete  4) Find  5) List all");
    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Name: "); var n = Console.ReadLine()!;
            Console.Write("Email: "); var e = Console.ReadLine()!;
            Console.Write("Roll number: "); var r = Console.ReadLine()!;
            var student = studentService.AddStudent(n, e, r);
            Console.WriteLine($"Added student #{student.Id}.");
            break;

        case "2":
            Console.Write("Student id: "); var editId = int.Parse(Console.ReadLine()!);
            Console.Write("New name: "); var nn = Console.ReadLine()!;
            Console.Write("New email: "); var ne = Console.ReadLine()!;
            studentService.UpdateStudent(editId, nn, ne);
            Console.WriteLine("Updated.");
            break;

        case "3":
            Console.Write("Student id: "); studentService.DeleteStudent(int.Parse(Console.ReadLine()!));
            Console.WriteLine("Deleted.");
            break;

        case "4":
            Console.Write("Keyword: ");
            foreach (var s in studentService.Search(Console.ReadLine() ?? ""))
                Console.WriteLine($"[{s.Id}] {s.Name} — {s.RollNumber}");
            break;

        case "5":
            foreach (var s in studentService.GetAllStudents())
                Console.WriteLine($"[{s.Id}] {s.Name} — {s.RollNumber} ({s.BorrowedBooksIds.Count} borrowed)");
            break;
    }
}

// ---------- Librarian menu ----------
void LibrarianMenu()
{
    Console.WriteLine("1) Add  2) Edit  3) Delete  4) Find  5) List all");
    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Name: "); var n = Console.ReadLine()!;
            Console.Write("Email: "); var e = Console.ReadLine()!;
            Console.Write("Staff code: "); var c = Console.ReadLine()!;
            var librarian = librarianService.AddLibrarian(n, e, c);
            Console.WriteLine($"Added librarian #{librarian.Id}.");
            break;

        case "2":
            Console.Write("Librarian id: "); var editId = int.Parse(Console.ReadLine()!);
            Console.Write("New name: "); var nn = Console.ReadLine()!;
            Console.Write("New email: "); var ne = Console.ReadLine()!;
            librarianService.UpdateLibrarian(editId, nn, ne);
            Console.WriteLine("Updated.");
            break;

        case "3":
            Console.Write("Librarian id: "); librarianService.DeleteLibrarian(int.Parse(Console.ReadLine()!));
            Console.WriteLine("Deleted.");
            break;

        case "4":
            Console.Write("Keyword: ");
            foreach (var l in librarianService.Search(Console.ReadLine() ?? ""))
                Console.WriteLine($"[{l.Id}] {l.Name} — {l.StaffCode}");
            break;

        case "5":
            foreach (var l in librarianService.GetAllLibrarians())
                Console.WriteLine($"[{l.Id}] {l.Name} — {l.StaffCode}");
            break;
    }
}

// ---------- Borrow / return / overdue ----------
void BorrowFlow()
{
    Console.Write("Student id: "); var sid = int.Parse(Console.ReadLine()!);
    var student = studentService.GetStudent(sid) ?? throw new MemberNotFoundException(sid);
    Console.Write("Book id: "); var bid = int.Parse(Console.ReadLine()!);
    libraryService.BorrowBook(bid, student, sid);
}

void ReturnFlow()
{
    Console.Write("Student id: "); var sid = int.Parse(Console.ReadLine()!);
    var student = studentService.GetStudent(sid) ?? throw new MemberNotFoundException(sid);
    Console.Write("Book id: "); var bid = int.Parse(Console.ReadLine()!);
    libraryService.ReturnBook(bid, student);
}

void ShowOverdue()
{
    var overdue = libraryService.GetOverdueLoans().ToList();
    if (overdue.Count == 0) { Console.WriteLine("No overdue books."); return; }

    foreach (var loan in overdue)
    {
        var book = bookService.GetBook(loan.BookId);
        Console.WriteLine($"Overdue: '{book?.Title}' — due {loan.DueDate:d}, borrowed by student #{loan.BorrowerId}");
    }
}