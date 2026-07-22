using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Exceptions;

public class MemberNotFoundException : Exception
{
    public MemberNotFoundException(int id) : base($"No member found with id:{id}")
    { }

}