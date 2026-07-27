using Microsoft.EntityFrameworkCore;

namespace TodoApp.Data;



public class AppDbContext:DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem { Id = 1, Title = "Learn ASP.NET Core", Description = "Master the framework", Priority = 3 },
             new TodoItem { Id = 2, Title = "Build a Todo App", Description = "Create a complete project", Priority = 2 },
             new TodoItem { Id = 3, Title = "Write Documentation", Description = "Document the code", IsCompleted = true, CompletedAt = DateTime.Now.AddDays(-1), Priority = 1 }
            );
    }

}
