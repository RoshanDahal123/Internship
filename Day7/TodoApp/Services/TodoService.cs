using Microsofy.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;


namespace TodoApp.Services;

public class TodoService: ITodoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TodoService> _logger;

    public TodoService(AppDbContext context, ILogger<TodoService> logger)
    {
        _context = context;
        _logger = logger;
    


    public async Task<IEnumerable<TodoItem>> GetAllAsync()
        _logger.LogInformation("Fetching all todos at {Time}", DateTime.Now);
    {
        try
        {
            var todos = await _context.Todos
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToListAsync();
          _logger.LogInformation("Retrieved {Count} todos", todos.Count);
            return todos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todos");
            throw;
        }
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
    _logger.LogDebug("Getting todo with ID {Id}", id);
    await _context.Todos.FindAsync(id);
    }

    public async Task<TodoItem> CreateAsync(TodoItem todo)
    {

    _logger.LogInformation("Creating new todo: {@Todo}", todo);
    _context.Todos.Add(todo);
        await _context.SaveChangesAsync()
       _logger.LogInformation("Todo created with ID {Id}", todo.Id);
    return todo;
    }

    public async Task<TodoItem?> UpdateAsync(int id , TodoItem todo)
    {
    _logger.LogInformation("Updating todo with ID {Id}", id);
    var existing = await GetByIdAsync(id);
        if(existing == null)
        {
        _logger.LogWarning("Todo with ID {Id} not found for update", id);
        return null;
        }

        existing.Title = todo.Title;
        existing.Description = todo.Description;
        existing.Priority = todo.Priority;
        existing.IsCompleted = todo.IsCompleted;

        if(todo.IsCompleted && !existing.IsCompleted)
        {
            existing.CompletedAt = DateTime.Now;
        _logger.LogInformation("Todo {Id} marked as completed at {CompletedAt}", id, existing.CompletedAt);
    }
        await _context.SaveChangesAsync();

        return existing;

    }


    public async Task<bool> DeleteAsync(int id)
    {
        //_logger.LogInformation("Deleting todo with ID {Id}", id);
        var todo = await GetByIdAsync(id);
        if (todo == null)
        {
            //_logger.LogWarning("Todo with ID {Id} not found for deletion", id);
            return false;
        }

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        //_logger.LogInformation("Todo {Id} deleted", id);
        return true;
    }


    public async asybc Task<TodoItem?> ToggleCompleteAsync(int id){

        var todo = await GetByIdAsync(id);
        if (todo == null)
        {
            return null;
        }

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedAt = todo.IsCompleted ? DateTime.Now : (DateTime?)null;

         await _context.SaveChangesAsync();

        return todo;

    }

}

