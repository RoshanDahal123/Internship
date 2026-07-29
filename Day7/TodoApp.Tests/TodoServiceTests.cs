using NUnit.Compatibility;
using TodoApp.Models;

namespace TodoApp.Tests;
//MSTestEquivalent:[TestClass]
[TestFixture]
public class TodoServiceTests: TodoServiceTestsBase
{
    [Test] // MSTest equivalent:[TestMethod]

    public async Task CreateAsync_ValidTodo_AddsToDatabase()
    {
        //Arrange
        var todo = new TodoItem
        {
            Title = "Learning NUnit",
            Description = "Study unit testing",
            Priority = 2
        };

        //Act 
        var created = await _service.CreateAsync(todo);
        //Assert
        Assert.That(created.Id, Is.GreaterThan(0));//Ef assigned a realId
        var fromDb = await _context.Todos.FindAsync(created.Id);
        Assert.That(fromDb,Is.Not.Null);
        Assert.That(fromDb!.Title, Is.EqualTo("Learning NUnit"));
    }
    [Test]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange — nothing to set up, database is empty

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.That(result, Is.Null);
    }


    [Test]

    public async Task DeleteAsync_ExistingTodo_RemoveItAndReturnsTrue()
    {
        //arrange
        var todo = new TodoItem { Title = "Temp Task" };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        //Act

        var result = await _service.DeleteAsync(todo.Id);

        //Assert

        Assert.That(result, Is.True);
        var fromDb = await _context.Todos.FindAsync(todo.Id);
        Assert.That(fromDb, Is.Null);
    }

    [Test]
    public async Task DeleteAsync_NonExistentId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        Assert.That(result, Is.False);
    }


    [Test]
    public async Task UpdateAsync_MarkingInCompleteTodoAsCompleted_SetsCompletedAt()
    {
        //arrange
        var todo = new TodoItem { Title = "Original", IsCompleted = false };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var updateData = new TodoItem
        {
            Id = todo.Id,
            Title = "Original",
            Description = "",
            Priority = 1,
            IsCompleted = true // marking it complete
        };

        // Act
        var result = await _service.UpdateAsync(todo.Id, updateData);

        //Assert

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsCompleted, Is.True);
        Assert.That(result.CompletedAt, Is.Not.Null);// this is exactly waht was broken

    }

    [Test]
    public async Task UpdateAsync_AlreadyCompletedTodo_DoesNotChangeCompletedAt()
    {
        // Arrange
        var originalCompletedDate = new DateTime(2026, 1, 1);
        var todo = new TodoItem { Title = "Already done", IsCompleted = true, CompletedAt = originalCompletedDate };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var updateData = new TodoItem { Id = todo.Id, Title = "Already done", IsCompleted = true, Priority = 1 };

        //act 

        var result = await _service.UpdateAsync(todo.Id, updateData);
        //Assert

        Assert.That(result!.CompletedAt,Is.EqualTo(originalCompletedDate));
    }

    [Test]
    public async Task ToggleCompleteAsync_InCompleteTodo_MarksCompletedSetsDate()
    {
        //arrange

        var todo = new TodoItem { Title="Task", IsCompleted = false };
        _context.Todos.Add(todo);

        await _context.SaveChangesAsync();

       
        //act
        var result = await _service.ToggleCompleteAsync(todo.Id);


        //assert

        Assert.That(result!.IsCompleted, Is.True);

        Assert.That(result.CompletedAt, Is.Not.Null);
    }

    [Test]
    public async Task ToggleCompleteAsync_CompletedTodo_MarksIncompleteAndClearsDate()
    {
        // Arrange
        var todo = new TodoItem { Title = "Task", IsCompleted = true, CompletedAt = DateTime.Now };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ToggleCompleteAsync(todo.Id);

        // Assert
        Assert.That(result!.IsCompleted, Is.False);
        Assert.That(result.CompletedAt, Is.Null);
    }
    [Test]
    public async Task GetAllAsync_MultipleTodos_OrdersByPriorityDescendingThenCreatedAtAscending()
    {
        // Arrange
        var low = new TodoItem { Title = "Low priority", Priority = 1, CreatedAt = new DateTime(2026, 1, 1) };
        var high = new TodoItem { Title = "High priority", Priority = 3, CreatedAt = new DateTime(2026, 1, 2) };
        var medium = new TodoItem { Title = "Medium priority", Priority = 2, CreatedAt = new DateTime(2026, 1, 3) };
        _context.Todos.AddRange(low, high, medium);
        await _context.SaveChangesAsync();

        // Act
        var result = (await _service.GetAllAsync()).ToList();

        // Assert — high priority should come first
        Assert.That(result[0].Title, Is.EqualTo("High priority"));
        Assert.That(result[1].Title, Is.EqualTo("Medium priority"));
        Assert.That(result[2].Title, Is.EqualTo("Low priority"));
    }


}
