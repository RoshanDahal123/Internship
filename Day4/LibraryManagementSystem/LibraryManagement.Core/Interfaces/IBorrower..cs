using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Interfaces;

//Interface= a contract. Any thing that can borrow books can implements this,

//regardless of what kind of Person it is

public interface IBorrower
{
    int MaxBooksAllowed { get; }
    List<int> BorrowedBooksIds { get; }
}
