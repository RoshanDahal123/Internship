## Dependency Injection 

The core idea: DI container

builder.Services is a container 
that ASP.NET Core uses to manage object creation. Instead of your

classes new AppDbContext(...) or new TodoService(...) themselves, 
you register these types up front, and the framework hands them to whatever 
needs them automatically — this is "dependency injection."

## Line 1: AddDbContext<AppDbContext>

````
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));
    ````