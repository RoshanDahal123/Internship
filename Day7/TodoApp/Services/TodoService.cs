using Microsofy.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;


namespace TodoApp.Services;

public class TodoService: ITodoService
{
    private readonly AppDbContext _context;
    
    public TodoService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        try
        {
            var todos = await _context.Todos
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToListAsync();

            return todos;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error fetching todos");
            throw;
        }
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        await _context.Todos.FindAsync(id);
    }

    public async Task<TodoItem> CreateAsync(TodoItem todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<TodoItem?> UpdateAsync(int id , TodoItem todo)
    {
        var existing = await GetByIdAsync(id);
        if(existing == null)
        {
            return null;
        }

        existing.Title = todo.Title;
        existing.Description = todo.Description;
        existing.Priority = todo.Priority;
        existing.IsCompleted = todo.IsCompleted;

        if(todo.IsCompleted && !existing.IsCompleted)
        {
            existing.CompletedAt = DateTime.Now;

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

