
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Models;

// Abstraction: Person only defines what every "person" in the system has in common.
// It can never be instantiated on its own — only through a concrete subtype.

public abstract class Person
{
    //Encapsulation:backing state is private/protected; exposed via controlled 
    //properties

    public int Id { get; }
    public string Name { get; protected set; }
    public string Email { get; protected set; }

    protected Person(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email =email;
    }
    //Polymorphism: every subtype overrides this to describe itself differently

    public abstract string GetRole();
    public override string ToString() => $"[{Id}] {Name} ({GetRole()})";



}