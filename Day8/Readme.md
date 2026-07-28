# Today's Learning Log — ASP.NET Core + EF Core + Serilog

**Project:** TodoApp (Day 7 Internship)
**Stack:** ASP.NET Core, EF Core, SQL Server, Serilog, Swashbuckle (Swagger)

This document summarizes the concepts covered today while building and debugging the TodoApp API.

---

## 1. Swashbuckle 10.x Namespace Change

Swashbuckle.AspNetCore 10.x upgraded its dependency on `Microsoft.OpenApi` to the 2.x line. In that version, Microsoft moved OpenAPI types (like `OpenApiInfo`) **out of** `Microsoft.OpenApi.Models` and **into** `Microsoft.OpenApi`.

**Before (breaks on .NET 10 / Swashbuckle 10.x):**
```csharp
c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { ... });
```

**After (correct):**
```csharp
using Microsoft.OpenApi;

c.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Todo API",
    Version = "v1",
    Description = "A simple Todo API built with ASP.NET Core"
});
```

**Takeaway:** package major-version bumps can silently move types to new namespaces — always check release notes / breaking changes when a `CS0234` (namespace not found) error appears out of nowhere.

---

## 2. SQL Server Connection Strings

Connection strings use `key=value` pairs separated by `;`. A common mistake is typing `:` (JSON's separator) instead of `=` (the connection string's separator) when editing `appsettings.json`.

**Broken:**
```json
"DefaultConn": "Server:MrWhite\\SQLEXPRESS;Database=TodoItem;..."
```

**Fixed:**
```json
"DefaultConn": "Server=MrWhite\\SQLEXPRESS;Database=TodoItem;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
```

**Note:** in JSON, `\` is an escape character, so a single backslash in a server instance name (`MrWhite\SQLEXPRESS`) must be written as `\\` inside the string.

---

## 3. What Are Migrations, and Why Do We Need Them?

- Your **C# model classes** (`TodoItem`, `AppDbContext`) describe what your data *should* look like.
- Your **actual database** is a separate thing living in SQL Server.
- Nothing keeps these two in sync automatically — that's what migrations are for.

**`EnsureCreated()`** — creates tables from your model, but only once. If the database already exists, it does nothing on later changes. Your model and database can silently drift apart.

**Migrations** — small generated C# files describing the *difference* between your model and the database's last known state (e.g. "add this column"). They are:
- **Incremental** — each migration only describes what changed.
- **Repeatable** — every environment (dev, staging, prod) can apply the same sequence and end up identical.
- **Version-controlled** — a full history of schema changes lives in your codebase alongside your model changes.

**Commands used:**
```bash
dotnet ef migrations add InitialCreate   # generates the migration file
dotnet ef database update                # applies it to the real database
```

In `Program.cs`, this happens automatically on startup via:
```csharp
context.Database.Migrate();
```

---

## 4. `HasData` and the "Model Changes Every Time It's Built" Error

**Error:**
```
PendingModelChangesWarning: The model for context 'AppDbContext' changes
each time it is built.
```

**Cause:** any *dynamic* value feeding into seed data (`HasData`) — either directly, or indirectly through a property's default value.

```csharp
// Direct example — dynamic value in HasData itself
new TodoItem { ..., CompletedAt = DateTime.Now.AddDays(-1) }

// Indirect example — dynamic default on the model
public DateTime CreatedAt { get; set; } = DateTime.Now;
```

Since `HasData` values get baked into the migration as static, literal data, EF needs the model to produce the *exact same value* every time it's built in order to compare it against the migration. `DateTime.Now` and `Guid.NewGuid()` never produce the same value twice — so EF thinks the model is perpetually "different."

**Fix:** always use fixed, hardcoded values in seed data — override any dynamic defaults explicitly for seeded rows:

```csharp
modelBuilder.Entity<TodoItem>().HasData(
    new TodoItem { Id = 1, Title = "Learn ASP.NET Core", Description = "Master the framework", Priority = 3, CreatedAt = new DateTime(2026, 1, 1) },
    new TodoItem { Id = 2, Title = "Build a Todo App", Description = "Create a complete project", Priority = 2, CreatedAt = new DateTime(2026, 1, 1) },
    new TodoItem { Id = 3, Title = "Learn C# advanced function", Description = "Learn the basic foundations", Priority = 1, CreatedAt = new DateTime(2026, 1, 1) }
);
```

---

## 5. Serilog Logging — Line by Line

```csharp
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/todoapp-.txt",
           rollingInterval: RollingInterval.Day,
           outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();
```

| Line | What it does |
|---|---|
| `new LoggerConfiguration()` | Starts building Serilog's configuration (nothing happens until `.CreateLogger()`). |
| `.ReadFrom.Configuration(builder.Configuration)` | Also reads Serilog settings from `appsettings.json`, so log levels can be tuned per environment without recompiling. |
| `.Enrich.FromLogContext()` | Allows extra properties (like request path/method) to be attached to logs automatically within a scope — used by `UseSerilogRequestLogging()`. |
| `.WriteTo.Console(...)` | A **sink** — writes logs to the terminal, useful while developing. |
| `.WriteTo.File("logs/todoapp-.txt", rollingInterval: RollingInterval.Day, ...)` | A second sink — writes to a file, starting a new file each day (e.g. `todoapp-20260128.txt`) so files stay manageable. |
| `outputTemplate` | Controls the exact text format: timestamp, log level (`{Level:u3}` = 3-letter abbreviation like `INF`/`WRN`/`ERR`), message, structured properties as JSON, and exception stack trace if present. |
| `.CreateLogger()` | Finalizes the configuration and builds the actual logger, assigned to the static `Log.Logger`. |
| `builder.Host.UseSerilog()` | Plugs Serilog into ASP.NET Core's built-in logging system, so every `_logger.LogInformation(...)` call in the app is routed through Serilog's sinks. |

**Structured logging** is the key reason to prefer Serilog over `Console.WriteLine`:

```csharp
_logger.LogInformation("Updating todo with ID {Id}", id);
```

`{Id}` is not string interpolation — it's a **named structured property**, stored separately from the message text. This means log data can later be queried (e.g. "show every log where Id = 42") if using a structured sink like Seq or Elasticsearch.

**Log levels** (least → most severe): `Trace → Debug → Information → Warning → Error → Critical`. Used today: `LogInformation` for normal flow, `LogWarning` for recoverable issues (e.g. "Todo not found for update").

---

## 6. Dependency Injection (DI) — Why It's Needed

**The problem without DI:**

```csharp
public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService()
    {
        _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=MrWhite\\SQLEXPRESS;Database=TodoItem;...")
            .Options);
    }
}
```

- **Hardcoded config** — the connection string is duplicated in every class that needs a database.
- **Untestable** — the constructor forces a real database connection; no way to substitute a fake one for unit tests.
- **No control over lifetime** — the class decides for itself when to create/destroy its dependencies, with no coordination across the app.

**The DI fix — "ask for it, don't build it":**

```csharp
public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context) // just ask for it
    {
        _context = context;
    }
}
```

`TodoService` no longer knows *how* `AppDbContext` is built or configured — it just declares what it needs. The DI container is configured once, in `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

builder.Services.AddScoped<ITodoService, TodoService>();
```

**Analogy:** a chef doesn't grow their own vegetables before cooking — they ask the kitchen for ingredients. DI is that supply chain for objects: one place configures how things are built, unlimited places can use them.

**Lifetimes:**

| Lifetime | Instance created | Typical use |
|---|---|---|
| Transient | New instance every request for it | Lightweight, stateless services |
| Scoped | One instance per HTTP request | `DbContext` and anything depending on it |
| Singleton | One instance for the app's whole lifetime | Caching, configuration |

`AppDbContext` and `TodoService` are both `Scoped` so that everything within a single HTTP request shares the same database context and change tracking.

---

## 7. Interfaces — Why `ITodoService` Instead of `TodoService` Directly

An interface is a **contract**, not an implementation — it says "anything claiming to be this must have these methods," without saying how they work internally.

```csharp
builder.Services.AddScoped<ITodoService, TodoService>();
```

```csharp
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService; // depends on the CONTRACT

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }
}
```

**Why this matters:**

1. **Testing** — a `FakeTodoService : ITodoService` can be substituted in tests, with no real database required. Impossible if the controller depended on the concrete `TodoService` class directly.
2. **Swappability** — a new implementation (e.g. `CachedTodoService`) can replace the old one by changing a single line in `Program.cs`. Every controller using `ITodoService` keeps working unchanged.
3. **Decoupling** — the controller doesn't need to know EF Core, SQL Server, or any implementation detail — only the method signatures it's promised.

**Analogy:** a power outlet. You don't care whether electricity comes from solar or a generator — you just plug into the standard socket shape (the interface). The interface is the socket; `TodoService` is one particular power source behind the wall.

---

## 8. Real Bug Found and Fixed Today — `UpdateAsync`

**The bug:**

```csharp
existing.IsCompleted = todo.IsCompleted;

if (todo.IsCompleted && !existing.IsCompleted) // always false — existing.IsCompleted was just overwritten above
{
    existing.CompletedAt = DateTime.Now;
}
```

By the time the `if` check runs, `existing.IsCompleted` had already been overwritten with `todo.IsCompleted` on the line above — so `!existing.IsCompleted` could never be true when `todo.IsCompleted` was true. `CompletedAt` was never being set.

**The fix — capture the original state before overwriting it:**

```csharp
public async Task<TodoItem?> UpdateAsync(int id, TodoItem todo)
{
    _logger.LogInformation("Updating todo with ID {Id}", id);
    var existing = await GetByIdAsync(id);
    if (existing == null)
    {
        _logger.LogWarning("Todo with ID {Id} not found for update", id);
        return null;
    }

    bool wasCompleted = existing.IsCompleted; // save old state first

    existing.Title = todo.Title;
    existing.Description = todo.Description;
    existing.Priority = todo.Priority;
    existing.IsCompleted = todo.IsCompleted;

    if (todo.IsCompleted && !wasCompleted) // compare against the saved old value
    {
        existing.CompletedAt = DateTime.Now;
        _logger.LogInformation("Todo {Id} marked as completed at {CompletedAt}", id, existing.CompletedAt);
    }

    await _context.SaveChangesAsync();

    return existing;
}
```

**Lesson:** when comparing "old vs new" state before mutating an object, always capture the old value into a local variable first — mutating fields in place destroys the information needed for the comparison.

---

## 9. Swagger UI — Custom Font Size

Swashbuckle allows injecting custom CSS into the Swagger UI page.

```csharp
app.UseStaticFiles(); // must be registered before Swagger UI serves its page

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
        c.InjectStylesheet("/swagger-custom.css");
    });
}
```

```css
/* wwwroot/swagger-custom.css */
.swagger-ui {
  font-size: 16px;
}
```

---

## 10. Migration Startup Block — Line by Line

```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}
```

`AppDbContext` is registered as **Scoped**, meaning it's meant to live for one HTTP request. But this code runs at *startup*, before any request exists. `app.Services.CreateScope()` manually creates a temporary scope so a scoped service can be safely resolved outside of a real request.

- `scope.ServiceProvider.GetRequiredService<AppDbContext>()` — asks the DI container for an `AppDbContext`, the same way a controller would get one via constructor injection. `GetRequiredService` (vs `GetService`) throws immediately if the service isn't registered — useful at startup, since a missing registration is a real config bug you want to know about right away.
- `context.Database.Migrate()` — checks `__EFMigrationsHistory`, compares it against migration files, and applies anything not yet run.
- The `using` block disposes the temporary `AppDbContext` once migration is done, so its database connection isn't held open for the app's entire lifetime.

---

## 11. How `TodoController` Gets Registered and Instantiated

Controllers are never registered by name (you never wrote `AddScoped<TodoController>()`). Instead:

1. **`builder.Services.AddControllers()` / `AddControllersWithViews()`** register the whole MVC/API framework, including a **controller factory** that knows how to find and build controller classes.
2. **Discovery by convention** — ASP.NET Core scans the project's compiled assembly for any public class inheriting `ControllerBase`/`Controller` (or named `*Controller`). `TodoController : ControllerBase` and `HomeController : Controller` are both found this way automatically.
3. **Routing** — `app.MapControllers()` activates attribute routing (`[Route]`, `[HttpGet]`, etc.) used by `TodoController`; `app.MapControllerRoute(...)` sets up conventional `{controller}/{action}/{id?}` routing used by `HomeController`'s views.
4. **Instantiation via DI** — once routing knows which controller should handle a request, the controller factory inspects its constructor (e.g. `TodoController(ITodoService todoService, ILogger<TodoController> logger)`) and asks the DI container to resolve each parameter — the same registrations used everywhere else (`AddScoped<ITodoService, TodoService>()`).
5. The controller instance handles that one request, then is disposed.

**Key insight:** controllers are *discovered* by convention and *instantiated* through the exact same DI system as any other service — no separate wiring needed.

---

## 12. Fixing the MVC UI — Empty `Create.cshtml` / `Edit.cshtml`

**The bug:** `TodoController` (the API) worked fine in Swagger, but the browser UI (served by `HomeController`) appeared broken. The cause wasn't in any C# code — `Create.cshtml` and `Edit.cshtml` were empty scaffold stubs with no form in them at all, so those pages rendered nothing.

**File roles clarified:**

| File | Role |
|---|---|
| `TodoController.cs` | API controller — returns JSON, used by Swagger/programs |
| `HomeController.cs` | UI controller — serves the actual browser pages, using the same `ITodoService` |
| `Index.cshtml` / `Delete.cshtml` | Already fully built |
| `Create.cshtml` / `Edit.cshtml` | Were empty — this was the actual bug |
| `ExceptionMiddleware.cs` | Global handler catching unhandled exceptions and returning a clean JSON error instead of a raw stack trace |

**Fix:** built out both forms using ASP.NET Core tag helpers, matching the existing Bootstrap/Font Awesome style:

```cshtml
@model TodoItem
<form asp-action="Create" method="post">
    <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>
    <div class="mb-3">
        <label asp-for="Title" class="form-label"></label>
        <input asp-for="Title" class="form-control" />
        <span asp-validation-for="Title" class="text-danger"></span>
    </div>
    <!-- Description, Priority (select), IsCompleted (Edit only) fields follow the same pattern -->
</form>
```

- **`asp-for`** binds an input directly to a model property, auto-generating the correct `name`/`id` so model binding works on submit.
- **`asp-validation-for`** automatically displays validation errors from the model's `[Required]`/`[StringLength]` attributes.
- `Edit.cshtml` only posts `Id`, `Title`, `Description`, `Priority`, `IsCompleted` — no hidden fields for `CreatedAt`/`CompletedAt`, since `TodoService.UpdateAsync` never touches those two on `existing` from the incoming `todo` argument.

**Lesson:** a "not working" UI isn't always a C# bug — an empty or missing view file will silently render a blank page with no exception thrown, since Razor has nothing to complain about.

---

## 13. Unit Testing with NUnit

**What a unit test is:** a small, automated check that calls one method in isolation (no real database, no running server) and verifies it did what was expected. The `UpdateAsync` bug found earlier — `CompletedAt` never being set — is exactly the kind of regression a unit test would have caught immediately, without needing to manually test through Swagger.

**Framework choice:** NUnit was used here; MSTest is nearly identical (attribute names differ slightly, noted below); xUnit is the modern default for new ASP.NET Core projects.

### Project setup (Visual Studio)

1. Right-click the **Solution** → `Add` → `New Project` → **NUnit Test Project** → name it `TodoApp.Tests`.
2. Right-click `TodoApp.Tests` → `Add` → `Project Reference` → check `TodoApp`.
3. Add two NuGet packages to the test project:
   ```
   dotnet add package Microsoft.EntityFrameworkCore.InMemory
   dotnet add package Moq
   ```
   - **EF Core InMemory** — runs `AppDbContext` against a fake in-memory database instead of real SQL Server: fast, isolated, no setup.
   - **Moq** — creates a fake `ILogger<TodoService>` to satisfy the constructor, since real logging isn't needed in tests.

### Arrange-Act-Assert pattern

Every test follows this shape, with a naming convention (`MethodName_Scenario_ExpectedResult`) that makes failures self-explanatory:

```csharp
[Test]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange — set up data/objects needed
    // Act — call the method being tested
    // Assert — check the result matches expectations
}
```

### Shared test setup (base class)

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApp.Data;
using TodoApp.Services;

namespace TodoApp.Tests;

public class TodoServiceTestsBase
{
    protected AppDbContext _context = null!;
    protected TodoService _service = null!;

    [SetUp] // MSTest: [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        _context = new AppDbContext(options);
        var mockLogger = new Mock<ILogger<TodoService>>();
        _service = new TodoService(_context, mockLogger.Object);
    }

    [TearDown] // MSTest: [TestCleanup]
    public void Cleanup() => _context.Dispose();
}
```

`[SetUp]` runs before every test, giving each one a fresh, isolated in-memory database (via a unique `Guid` name) so tests can never leak data into each other. `[TearDown]` cleans up after each test.

### Key tests written against `TodoService`

```csharp
[TestFixture] // MSTest: [TestClass]
public class TodoServiceTests : TodoServiceTestsBase
{
    [Test] // MSTest: [TestMethod]
    public async Task CreateAsync_ValidTodo_AddsToDatabase()
    {
        var todo = new TodoItem { Title = "Learn NUnit", Priority = 2 };
        var created = await _service.CreateAsync(todo);
        Assert.That(created.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(999);
        Assert.That(result, Is.Null);
    }

    // Regression test for the CompletedAt bug found and fixed earlier
    [Test]
    public async Task UpdateAsync_MarkingIncompleteTodoAsCompleted_SetsCompletedAt()
    {
        var todo = new TodoItem { Title = "Original", IsCompleted = false };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var updateData = new TodoItem { Id = todo.Id, Title = "Original", IsCompleted = true, Priority = 1 };
        var result = await _service.UpdateAsync(todo.Id, updateData);

        Assert.That(result!.CompletedAt, Is.Not.Null); // this is exactly what was broken before the fix
    }

    [Test]
    public async Task ToggleCompleteAsync_CompletedTodo_MarksIncompleteAndClearsDate()
    {
        var todo = new TodoItem { Title = "Task", IsCompleted = true, CompletedAt = DateTime.Now };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var result = await _service.ToggleCompleteAsync(todo.Id);

        Assert.That(result!.IsCompleted, Is.False);
        Assert.That(result.CompletedAt, Is.Null);
    }
}
```

**Why not just test happy paths:** deliberately covered not-found cases (`GetByIdAsync_NonExistentId_ReturnsNull`), the exact bug fixed earlier (as a permanent regression guard), and "don't break what already works" cases (e.g. an already-completed todo's `CompletedAt` shouldn't change on update).

**Running tests:** `Test` menu → `Run All Tests`, or `Test Explorer` (`Ctrl+E, T`). Green = passing, red = failing with an expected-vs-actual message.

---

## 14. Debugging Techniques in Visual Studio

| Technique | How | Use |
|---|---|---|
| Breakpoints | `F9` or click the left margin | Pause execution at a specific line |
| Conditional breakpoints | Right-click a breakpoint | Only break when a condition is true (e.g. `id == 5`) |
| Step Over | `F10` | Run the current line without entering method calls |
| Step Into | `F11` | Enter the method being called |
| Step Out | `Shift+F11` | Finish the current method, return to caller |
| Watch window | `Debug → Windows → Watch` | Pin variables/expressions to track while stepping |
| Immediate window | `Debug → Windows → Immediate` | Execute C# expressions live while paused |
| Exception Settings | `Ctrl+Alt+E` → check CLR Exceptions | Break the instant any exception is thrown, even if later caught |

**Applied to the `UpdateAsync` bug:** setting a breakpoint on `if (todo.IsCompleted && !wasCompleted)` and hovering over `wasCompleted` vs `existing.IsCompleted` right before that line would have shown both were already equal — revealing the overwrite bug directly, without needing a test to surface it.

---

## Summary

| Concept | Key Idea |
|---|---|
| Swashbuckle namespace change | Package major versions can move types to new namespaces |
| Connection strings | `key=value` pairs, not `key:value` |
| Migrations | Keep the database schema in sync with the C# model, incrementally and repeatably |
| `HasData` pitfalls | Seed data must be fully static/deterministic |
| Serilog | Structured logging + multiple sinks (console, file) via one configuration |
| Dependency Injection | Classes declare what they need; a central container builds and supplies it |
| Interfaces | Define a contract so implementations can be swapped, tested, and decoupled |
| `UpdateAsync` bug | Capture old state before overwriting fields you need to compare against |
| Migration startup block | Manually creates a DI scope to resolve `AppDbContext` outside a real request |
| Controller registration | Controllers are discovered by convention, instantiated via the same DI container |
| Empty MVC views | A missing/empty `.cshtml` file renders blank with no exception — not always a C# bug |
| Unit Testing (NUnit) | Arrange-Act-Assert; EF InMemory + Moq for isolated, fast tests; regression tests guard fixed bugs |
| VS Debugging | Breakpoints, stepping, Watch/Immediate windows, and Exception Settings for live investigation |
