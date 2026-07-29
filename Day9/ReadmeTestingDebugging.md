# Unit Testing (NUnit vs MSTest) & Visual Studio Debugging

**Project:** TodoApp (Internship)
**Focus:** Unit testing fundamentals, NUnit vs MSTest comparison, and hands-on Visual Studio debugging techniques.

---

## Part 1: What Unit Testing Is, and Why It Matters

A **unit test** is a small, automated piece of code that calls one method in isolation and checks whether it did what was expected. "Isolation" means no real database, no running web server — just the method's logic being verified directly.

**Why this matters (a real example from this project):** an earlier bug in `TodoService.UpdateAsync` meant `CompletedAt` never got set when a todo was marked complete, because `existing.IsCompleted` was overwritten *before* being compared against its old value. A unit test asserting *"marking a todo complete should set CompletedAt"* would have failed immediately and pointed straight at the problem — without needing to manually test through Swagger and notice a null field.

---

## Part 2: NUnit vs MSTest — Full Comparison

| Concept | NUnit | MSTest |
|---|---|---|
| Test class | `[TestFixture]` (often optional) | `[TestClass]` (required) |
| Test method | `[Test]` | `[TestMethod]` |
| Runs before each test | `[SetUp]` | `[TestInitialize]` |
| Runs after each test | `[TearDown]` | `[TestCleanup]` |
| Runs once before all tests in class | `[OneTimeSetUp]` (can be instance method) | `[ClassInitialize]` (must be `static`) |
| Runs once after all tests in class | `[OneTimeTearDown]` (can be instance method) | `[ClassCleanup]` (must be `static`) |
| Skip a test | `[Ignore("reason")]` | `[Ignore]` |
| Parameterized test | `[TestCase(1, 2, 3)]` | `[DataRow(1, 2, 3)]` |
| Assertion style | Fluent: `Assert.That(x, Is.EqualTo(y))` | Classic: `Assert.AreEqual(y, x)` |
| Origin | Third-party, very mature, huge community | Built by Microsoft, ships with VS templates |

### Execution order example (both frameworks, same logic)

```
OneTimeSetUp / ClassInitialize   ← once, before any test
  SetUp / TestInitialize          ← before Test1
    Test1
  TearDown / TestCleanup          ← after Test1
  SetUp / TestInitialize          ← before Test2
    Test2
  TearDown / TestCleanup          ← after Test2
OneTimeTearDown / ClassCleanup   ← once, after all tests
```

### When to use `SetUp` vs `OneTimeSetUp`

- **`[SetUp]` / `[TestInitialize]`** — anything that must be **fresh per test**, e.g. a new in-memory `AppDbContext` so tests never leak data into each other.
- **`[OneTimeSetUp]` / `[ClassInitialize]`** — expensive, **shared, read-only** setup safe to reuse across tests (e.g. starting a `TestServer`). Never use this for a `DbContext` that tests will mutate — sharing it breaks test isolation.

**Key MSTest quirk:** `[ClassInitialize]`/`[ClassCleanup]` *must* be `static`, because they run before any instance of the test class exists (MSTest creates a new instance per test method). NUnit has no such restriction.

---

## Part 3: NUnit Project Setup (Visual Studio)

1. Right-click the **Solution** → `Add` → `New Project` → **NUnit Test Project** → name it `TodoApp.Tests`.
2. Right-click `TodoApp.Tests` → `Add` → `Project Reference` → check `TodoApp`.
3. Add NuGet packages:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.InMemory
   dotnet add package Moq
   ```
   - **EF Core InMemory** — runs `AppDbContext` against a fake in-memory database instead of real SQL Server: fast, isolated, no setup required.
   - **Moq** — creates a fake `ILogger<TodoService>` to satisfy the constructor without real logging.

**Checking project references:**
```bash
dotnet list reference          # run from inside the actual .csproj folder
```
If this says "no references" despite Solution Explorer showing one, you're likely running it from a parent/solution folder rather than the actual nested project folder (Visual Studio often creates `TodoApp.Tests/TodoApp.Tests/TodoApp.Tests.csproj`). Confirm directly by opening the `.csproj` in a text editor and looking for:
```xml
<ItemGroup>
  <ProjectReference Include="..\TodoApp\TodoApp.csproj" />
</ItemGroup>
```

**NUnit1032 build error** ("field should be Disposed in `[TearDown]`") — the default scaffolded `UnitTest1.cs` file is usually the cause; delete it once real tests exist, or add a `[TearDown]` that disposes any `IDisposable` field.

---

## Part 4: Arrange-Act-Assert Pattern

Every test follows this shape:

```csharp
[Test]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange — set up data/objects needed
    // Act — call the method being tested
    // Assert — check the result matches expectations
}
```

The naming convention (`MethodName_Scenario_ExpectedResult`) makes failing tests self-explanatory from the test name alone, without opening the test body.

---

## Part 5: Shared Test Setup (Base Class)

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

`[SetUp]` runs before every test, giving each a fresh, isolated in-memory database via a unique `Guid` name — tests can never leak data into each other. `[TearDown]` disposes the context after each test.

---

## Part 6: Real Tests Against `TodoService`

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

    [Test]
    public async Task DeleteAsync_ExistingTodo_RemovesItAndReturnsTrue()
    {
        var todo = new TodoItem { Title = "Temp task" };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteAsync(todo.Id);

        Assert.That(result, Is.True);
        Assert.That(await _context.Todos.FindAsync(todo.Id), Is.Null);
    }

    // Regression test for the CompletedAt bug found and fixed earlier in this project
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
    public async Task UpdateAsync_AlreadyCompletedTodo_DoesNotChangeCompletedAt()
    {
        var originalDate = new DateTime(2026, 1, 1);
        var todo = new TodoItem { Title = "Already done", IsCompleted = true, CompletedAt = originalDate };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var updateData = new TodoItem { Id = todo.Id, Title = "Already done", IsCompleted = true, Priority = 1 };
        var result = await _service.UpdateAsync(todo.Id, updateData);

        Assert.That(result!.CompletedAt, Is.EqualTo(originalDate)); // should NOT be overwritten
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

**Why not just test happy paths:** these tests deliberately cover not-found cases, the exact bug that was fixed (as a permanent regression guard), and "don't break what already works" cases.

**Running tests:** `Test` menu → `Run All Tests`, or `Test Explorer` (`Ctrl+E, T`). Green = passing, red = failing with an expected-vs-actual message.

---

## Part 7: Visual Studio Debugging Techniques

### Core toolkit

| Technique | How | Use |
|---|---|---|
| Breakpoints | `F9` or click the left margin | Pause execution at a specific line |
| Conditional breakpoints | Right-click a breakpoint → `Conditions...` | Only break when a condition is true (e.g. `id == 3`) |
| Step Over | `F10` | Run the current line without entering method calls |
| Step Into | `F11` | Enter the method being called |
| Step Out | `Shift+F11` | Finish the current method, return to caller |
| Locals window | Auto-visible while paused | Shows every variable currently in scope |
| Watch window | `Debug → Windows → Watch` | Pin variables/expressions to track continuously while stepping |
| Immediate window | `Debug → Windows → Immediate` | Execute C# expressions live, on demand, while paused |
| Call Stack | `Debug → Windows → Call Stack` | Shows the chain of method calls that led here; double-click a frame to jump to it |
| Exception Settings | `Ctrl+Alt+E` → check CLR Exceptions | Break the instant any exception is thrown, even if a `catch` block would later swallow it |

**`F5` vs `Ctrl+F5`:** `F5` = Start Debugging (breakpoints active). `Ctrl+F5` = Start Without Debugging (breakpoints ignored).

**Step Over vs Step Into rule of thumb:** use `F10` for framework/library calls (`_logger.LogInformation`, `_context.Todos.FindAsync`) — you don't need to step into their internals. Use `F11` only for calls into *your own* code you actually want to inspect.

### Example 1 — Simple: watching a variable-comparison bug

Debugging the `UpdateAsync` `CompletedAt` bug directly:

1. Breakpoint on `bool wasCompleted = existing.IsCompleted;`
2. `F10` past it.
3. Add `existing.IsCompleted` and `wasCompleted` to the Watch window.
4. Step through the following lines — watching both values side by side makes an incorrect comparison (e.g. comparing a field against itself after it's already been overwritten) visually obvious, rather than something you have to mentally trace.

### Example 2 — Complex: multi-layer bug across Controller → Service → EF Core

Scenario: *"Editing a todo's Priority sometimes doesn't save, with no error shown."*

1. **Isolate the reproduction case** with a conditional breakpoint: `id == 3` on the first line of `UpdateAsync`, so only that specific todo's update pauses execution.
2. **Watch EF Core's internal tracking state**, not just your own variables:
   ```
   todo.Priority
   existing.Priority
   _context.Entry(existing).State
   ```
   `_context.Entry(existing).State` shows EF's change-tracking status: `Unchanged`, `Modified`, `Added`, `Deleted`, or `Detached`.
3. **Step past `existing.Priority = todo.Priority;`** and watch the state flip `Unchanged → Modified` — this confirms EF correctly detected the change and that `existing` is a properly tracked instance (ruling out a "detached entity from a different `DbContext`" bug).
4. **Use the Call Stack** to jump back to the calling `HomeController.Edit` frame and confirm the incoming `todo.Priority` was correct *before* it even reached the service — ruling out a model-binding bug upstream in the view/form.
5. **Step past `await _context.SaveChangesAsync();`** and watch the state flip back to `Unchanged` — this confirms the `UPDATE` was actually generated and successfully committed to the database. If it *doesn't* return to `Unchanged` (stays `Modified`, or an exception is thrown instead), that's the signal something failed at the database layer.

**Full state lifecycle observed across this debugging session:**
```
Unchanged  →  (edit existing.Priority)  →  Modified  →  (SaveChangesAsync)  →  Unchanged
```
Each transition is direct proof of a step in the pipeline working correctly — not something inferred from reading code, but something observed happening live.

### Common Watch/Immediate window pitfalls

**"This expression causes side effects and will not be evaluated"**
Method calls like `_context.Entry(existing)` aren't plain field/property access — Visual Studio blocks them by default in the Watch window since it auto-re-evaluates on every step, and a method could have side effects.
- **Fix:** `Tools → Options → Debugging → General → Enable property evaluation and other implicit function calls`.
- **Alternative:** use the **Immediate Window** instead — it only runs once, when you explicitly press Enter, rather than re-running automatically on every step.

**`NullReferenceException` / `ArgumentNullException: Value cannot be null (Parameter 'entity')` when inspecting a variable**
This means the variable (e.g. `existing`) is genuinely `null` **at the exact line you're currently paused on** — Watch/Immediate expressions evaluate based on the current execution point, not "somewhere in this method."
- Check the **yellow arrow** in the editor's left margin to see exactly which line is currently paused.
- If you're paused *before* the assignment (e.g. still on `var existing = await GetByIdAsync(id);` itself), step past it with `F10` first.
- If you've genuinely stepped past the assignment and it's still `null`, that's a real bug worth investigating (e.g. the `id` being searched for doesn't exist in the database) — though note the code's own `if (existing == null) { ...; return null; }` guard would normally catch this and exit early before reaching further lines.

### Exception Settings — why it matters with global exception handling

Since `ExceptionMiddleware` catches *every* unhandled exception app-wide and returns a generic JSON error, the debugger's default behavior (only stopping on *uncaught* exceptions) means you'd never see it pause — the exception gets caught and logged before you can inspect it. Enabling **"break on all CLR exceptions"** (`Ctrl+Alt+E`) makes the debugger pause the instant an exception is thrown, before any `catch` block runs — letting you inspect the real state that caused it, right at the source, with the Call Stack showing you exactly which line triggered it.

---

## Summary

| Concept | Key Idea |
|---|---|
| Unit testing | Isolated, automated checks on one method at a time; catches regressions immediately |
| NUnit vs MSTest | Same concepts, different attribute names; NUnit uses fluent `Assert.That`, MSTest uses classic `Assert.AreEqual`-style methods |
| `SetUp` vs `OneTimeSetUp` | Per-test fresh state vs one-time shared, read-only setup |
| Arrange-Act-Assert | Structures every test; naming convention makes failures self-explanatory |
| EF Core InMemory + Moq | Enables fast, isolated service-layer tests without a real database or real logger |
| Regression tests | A test written specifically to guard a previously-fixed bug from reappearing |
| Breakpoints & stepping | `F9`/`F10`/`F11`/`Shift+F11` — the foundation of live investigation |
| Watch vs Immediate window | Watch auto-re-evaluates every step (can misfire on side-effecting calls); Immediate runs once, on demand |
| `_context.Entry(x).State` | Direct window into EF Core's change tracking — proves whether a change will actually be saved |
| Call Stack | Traces which layer (Controller → Service → EF Core) triggered the current point |
| Exception Settings | Needed to catch exceptions live when global middleware would otherwise swallow them silently |