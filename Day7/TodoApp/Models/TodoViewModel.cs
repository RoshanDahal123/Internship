namespace TodoApp.Models;
public class TodoViewModel
{
    public List<TodoItem> Todos { get; set; } = new();
    public int CompletedCount => Todos?.Count(t => t.IsCompleted) ?? 0;
    public int TotalCount => Todos?.Count ?? 0;
    public double CompletionRate => TotalCount > 0 ? (double)CompletedCount / TotalCount * 100 : 0;
}