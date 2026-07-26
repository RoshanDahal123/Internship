# 1. What EF Core actually is

 EF Core is an ORM (Object-Relational Mapper). It sits on top of ADO.NET (literally — internally it still uses SqlConnection, SqlCommand, etc.,
but you never touch those directly).

The core idea: instead of writing SQL strings and manually mapping DataReader columns to C# 
properties (like you've been doing), you work with C# objects and LINQ queries, and EF Core translates them into SQL automatically.

````

const string query = "SELECT Id, Name, Department, Salary FROM Employees WHERE Id = @Id";
using var cmd = new SqlCommand(query, conn);
cmd.Parameters.AddWithValue("@Id", id);
using var reader = await cmd.ExecuteReaderAsync();
if (await reader.ReadAsync())
{
    return new Employee { Id = reader.GetInt32(...), Name = reader.GetString(...), ... };
}
````

## Equivalent in EF Core:

```` csharp
var employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
````

That one line replaces the whole block — EF Core generates the SQL, opens the connection, runs it, maps the result back into an Employee object, and disposes everything, all internally.

## 2. Core building blocks of EF Core
1. Entity(your model class)

  Ef core calls the model class as entity - a class that maps toa a database table.

2. DbContext (raplave the earlier ADO.NET EmployeeRepository's cnnection logic)

  this is the heart of Ef core- it represents a session with tht databse and exposes your tables
  as queryable collecions.
  ````
  using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=MrWhite\\SQLEXPRESS;Database=Emp;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
````

** DbSet<Employee> Employees — this property represents your Employees table. Querying it is like querying the table.
** OnConfiguring — tells EF Core which database and provider to use (here, SQL Server, via the connection string).

## NuGet packages you need

````
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
````

## 3. Reading data — the part you asked about specifically

This is where EF core really shines over raw ADO.NET. Here are the main ways to read:

## Gett all rows
using var context = new AppDbContext();
List<Employee> employees= await context.Employees.ToListAsync();

## Get by primary key- the fastest most direct look up
Employee? emp = await context .Employees.FindAsync(id);

## c. Get with the filter.
List<Employee> itEmployees = await context.Employees.Where(e=>e.Department=="IT")
                                                   .ToListAsync();
 which is equivalent to Select * from employees where department ="IT"
 ## get a single matching row

 Employee? emp= await context.Employees.FirstOrDefaultAsync(e=>e.Id==id);

 ** FirstOrDefaultAsync — returns the first match, or null if none found.
 Doesn't complain if there are multiple matches. **

 **SingleOrDefaultAsync — same, but throws an exception if more than one row matches 
(use this when you expect exactly 0 or 1 result, like a unique Id lookup — enforces 
that assumption).**


## sorting

List<Employee> sorted = await contex.Employees.OrderByDescending(e=>e.Id).
.Skip(10).Take(10).ToListAsync();

context.Employees.Where(i=>u.id>500 || u.EmployeesId.EndsWith("2")).ORderBy(uu=> u.UrunAdi);

THEnBY
var sortedUsers = await dbContext.Users.OrederBy(u=>u.LastName).ThenBy(u=>u.FirstName).ToListAsync();


###porjections - selecting only specfic columns

var namesOnly= await context.Employees.SElect(e=>new{e.Id,e.Name}).ToListAsync();
 *** This is important for performance — EF Core is smart enough to generate SELECT Id, Name FROM Employees (not SELECT *) 
 when you project like this,
 since it knows you only need those two columns.***

 # 4. How this translates to SQL — deferred execution
 This is a crucial concept to understand. This line:
 ````
 var query = context.Employees.Where(e => e.Department == "IT");
 ````
 does not run any SQL yet. It just builds up a query expression (an IQueryable<Employee>). 
 The SQL only actually executes when you call something that forces evaluation:

 ````
 var list = await query.ToListAsync();   // 👈 THIS is when SQL runs
 ````
 Other things that trigger execution: .ToListAsync(), .FirstOrDefaultAsync(), .CountAsync(), 
 foreach over the query, etc.

This is called deferred execution / lazy query building — you can keep chaining .Where(),
.OrderBy() etc. and
EF Core combines them all into a single efficient SQL query, only sent once you actually 
ask for the results.

For migration we use the command

````
dotnet ef migrations add InitialCreate
//This looks at your DbContext/entity classes and generates a C# file describing the schema changes (a "migration") —
you can read and even hand-edit it.
dotnet ef database update //This applies pending migrations to the actual database.
````

````
//Later, when you change your model (say, add a PublishedYear property to Book):
dotnet ef migrations add AddPublishedYear
//EF diffs your current model against the last migration snapshot and generates only the incremental 
ALTER TABLE needed — your existing data stays intact.
dotnet ef database update
````

for this you need to install the 

````
dotnet tool install --global dotnet -ef
````



EF core vs LINQ to SQL

