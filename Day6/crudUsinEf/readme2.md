# LINQ to SQL vs. EF Core

A quick-reference comparison for choosing between, or understanding the difference between, Microsoft's two LINQ-based ORMs for .NET.

---

## Overview

| | **LINQ to SQL** | **EF Core** |
|---|---|---|
| Introduced | 2007 (.NET 3.5) | 2016 (successor to EF6) |
| Status | Legacy — no longer actively developed | Actively developed, current standard |
| Database support | SQL Server **only** | SQL Server, MySQL, PostgreSQL, SQLite, Oracle, Cosmos DB, and more |
| Recommended for new projects | ❌ No | ✅ Yes |
| Async/await support | ❌ None — synchronous only | ✅ Full async support (`...Async()` methods) |
| Schema migrations | ❌ Manual only | ✅ Built-in Migrations system |
| Relationship/navigation support | Basic | Rich (one-to-many, many-to-many, owned types, etc.) |
| LINQ query translation | Limited | Much more capable and mature |

---

## Core Concepts — Naming Differences

Both frameworks use the same underlying idea: a context class represents your database session, and a queryable property represents each table. The concepts map 1-to-1, but the class/method names differ.

| Concept | LINQ to SQL | EF Core |
|---|---|---|
| Context class | `DataContext` | `DbContext` |
| Table representation | `Table<Employee>` | `DbSet<Employee>` |
| Mapping | `[Table]` / `[Column]` attributes (explicit) | Convention-based, or Fluent API / attributes |
| Stage an insert | `InsertOnSubmit(entity)` | `Add(entity)` |
| Stage a delete | `DeleteOnSubmit(entity)` | `Remove(entity)` |
| Commit changes | `SubmitChanges()` | `SaveChanges()` / `SaveChangesAsync()` |
| Query all rows | `.ToList()` | `.ToList()` / `.ToListAsync()` |
| Find by primary key | `.FirstOrDefault(e => e.Id == id)` | `.Find(id)` / `.FindAsync(id)` |

---

## Setup Comparison

### LINQ to SQL

```csharp
using System.Data.Linq;
using System.Data.Linq.Mapping;

public class EmployeeDataContext : DataContext
{
    public EmployeeDataContext(string connectionString) : base(connectionString) { }

    public Table<Employee> Employees;
}

[Table(Name = "Employees")]
public class Employee
{
    [Column(IsPrimaryKey = true, IsDbGenerated = true)]
    public int Id { get; set; }

    [Column]
    public string Name { get; set; } = string.Empty;

    [Column]
    public string? Department { get; set; }

    [Column]
    public decimal Salary { get; set; }
}
```

### EF Core

```csharp
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public AppDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Department { get; set; }
    public decimal Salary { get; set; }
}
```

**Key difference:** EF Core can infer the primary key, table name, and column mappings purely from naming conventions (e.g., a property named `Id` is automatically the primary key). LINQ to SQL generally requires explicit `[Table]` / `[Column]` attributes on every mapped property.

---

## CRUD Comparison

### Create (Insert)

```csharp
// LINQ to SQL
var emp = new Employee { Name = "John Doe", Department = "IT", Salary = 55000 };
db.Employees.InsertOnSubmit(emp);
db.SubmitChanges();

// EF Core
var emp = new Employee { Name = "John Doe", Department = "IT", Salary = 55000 };
context.Employees.Add(emp);
await context.SaveChangesAsync();
```

### Read (all)

```csharp
// LINQ to SQL
var all = db.Employees.ToList();

// EF Core
var all = await context.Employees.ToListAsync();
```

### Read (filtered / by Id)

```csharp
// LINQ to SQL
var itEmployees = db.Employees.Where(e => e.Department == "IT").ToList();
var emp = db.Employees.FirstOrDefault(e => e.Id == id);

// EF Core
var itEmployees = await context.Employees.Where(e => e.Department == "IT").ToListAsync();
var emp = await context.Employees.FindAsync(id);
```

### Update

```csharp
// LINQ to SQL
var emp = db.Employees.FirstOrDefault(e => e.Id == id);
if (emp != null)
{
    emp.Salary = 65000;
    db.SubmitChanges();
}

// EF Core
var emp = await context.Employees.FindAsync(id);
if (emp != null)
{
    emp.Salary = 65000;
    await context.SaveChangesAsync();
}
```

### Delete

```csharp
// LINQ to SQL
var emp = db.Employees.FirstOrDefault(e => e.Id == id);
if (emp != null)
{
    db.Employees.DeleteOnSubmit(emp);
    db.SubmitChanges();
}

// EF Core
var emp = await context.Employees.FindAsync(id);
if (emp != null)
{
    context.Employees.Remove(emp);
    await context.SaveChangesAsync();
}
```

**Key difference:** the operations are conceptually identical — stage a change, then commit — but EF Core's methods are async throughout, while LINQ to SQL is fully synchronous with no async equivalents at all.

---

## Query Syntax vs. Method Syntax

Both frameworks support two equivalent ways of writing LINQ queries — this isn't a LINQ to SQL vs. EF Core difference, it's a general C#/LINQ feature available in both:

```csharp
// Method syntax
var result = context.Employees.Where(e => e.Department == "IT").ToList();

// Query syntax
var result = (from e in context.Employees
              where e.Department == "IT"
              select e).ToList();
```

Both compile down to the same underlying calls. Method syntax is more common in modern codebases.

---

## Why EF Core Replaced LINQ to SQL

1. **No async support** — LINQ to SQL blocks the calling thread on every database call, which hurts scalability, especially in web applications.
2. **SQL Server only** — no ability to target MySQL, PostgreSQL, SQLite, or other providers without a full rewrite.
3. **No migrations system** — schema changes must be scripted and applied manually.
4. **Weaker LINQ translation** — many LINQ expressions that work fine in EF Core don't translate to valid SQL in LINQ to SQL.
5. **No longer maintained** — Microsoft's investment has been in EF Core since around 2016; LINQ to SQL receives no new features or fixes.

---

## Summary Recommendation

| Situation | Recommendation |
|---|---|
| Starting a new project | Use **EF Core** |
| Maintaining an older codebase that already uses LINQ to SQL | Learn enough LINQ to SQL to work with it; consider migrating to EF Core if there's time/budget |
| Learning ORMs for the first time | Focus on **EF Core** — it's the current, actively supported standard |

LINQ to SQL is worth understanding conceptually (the patterns transfer almost directly), but it's not something to build new projects with today.