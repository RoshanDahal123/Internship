using TodoApp.Models;

namespace TodoApp.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task<TodoItem> CreateAsync(TodoItem todo);
    Task<TodoItem?> UpdateAsync(int id, TodoItem todo);
    Task<bool> DeleteAsync(int id);
    Task<TodoItem?> ToggleCompleteAsync(int id);
}