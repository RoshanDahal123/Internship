using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

//Inheritance:Student IS-A person, plus borrowing behavoiur via IBorrower

public class Student : Person, IBorrower
{
    public string RollNumber { get; }
    public int MaxBooksAllowed => 3;
    public List<int> BorrowedBooksIds { get; } = new();
    public Student (int id, string name, string email, string rollNumber) : base(id, name, email)
    {
        RollNumber = rollNumber;
    }
    public override string GetRole() => "Student";
   
    
}
