using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Models;

public class Librarian: Person
{
    public string StaffCode { get; }
    public Librarian(int id, string name, string email, string staffCode):base(
        id, name, email)
    {
        StaffCode = staffCode;
    }
    public override string GetRole() => "Librarian";
    
}
