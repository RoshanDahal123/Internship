



using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Data.Repositories;

namespace LibraryManagement.Services;

public class LibrarianService
{
    private readonly Repository<Librarian> _librarians;
    public LibrarianService(Repository<Librarian>librarians)=>_librarians=librarians;

    public Librarian AddLibrarian(string name, string email, string staffCode)
    {
        if (_librarians.GetAll().Any(l => l.StaffCode == staffCode))
            throw new DuplicateEntityException($"Staff code {staffCode} is already in use.");

        var librarian = new Librarian(_librarians.GetNextId(), name, email, staffCode);
        _librarians.Add(librarian);
        return librarian;
    }
    public void UpdateLibrarian(int id, string name, string email)
    {
        var librarian = _librarians.GetById(id) ?? throw new MemberNotFoundException(id);
        librarian.UpdateContactInfo(name, email);
        _librarians.Update(librarian);
    }

    public void DeleteLibrarian(int id)
    {
        var librarian= _librarians.GetById(id) ?? throw new MemberNotFoundException(id);
        _librarians.Remove(id);
    }


    public Librarian? GetLibrarian(int id) => _librarians.GetById(id);

    public IEnumerable<Librarian> GetAllLibrarians() => _librarians.GetAll();

    public IEnumerable<Librarian> Search(string keyword) =>
       _librarians.GetAll().Where(l =>
           l.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
           l.StaffCode.Contains(keyword, StringComparison.OrdinalIgnoreCase));


}