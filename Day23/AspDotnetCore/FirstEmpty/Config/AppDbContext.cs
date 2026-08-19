using Microsoft.EntityFrameworkCore;

namespace FirstEmpty.Config;


public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

}
