
using Microsoft.EntityFrameworkCore;

namespace MySqlCrudApp;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;
    public AppDbContext(string connectionString)
    {
        _connectionString= connectionString;
    }

    public DbSet<Employee> Employees { get; set; } = null!;//null forgiving operator

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

}