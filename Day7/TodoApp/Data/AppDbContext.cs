using Microsoft.EntityFrameworkCore;
using TodoApp.Models;



namespace TodoApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().HasData(
        new TodoItem { Id = 1, Title = "Learn ASP.NET Core", Description = "Master the framework", Priority = 3, CreatedAt = new DateTime(2026, 1, 1) },
        new TodoItem { Id = 2, Title = "Build a Todo App", Description = "Create a complete project", Priority = 2, CreatedAt = new DateTime(2026, 1, 1) },
        new TodoItem { Id = 3, Title = "Learn C# advanced function", Description = "Learn the basic foundations", Priority = 1, CreatedAt = new DateTime(2026, 1, 1) }
    );
    }

}
