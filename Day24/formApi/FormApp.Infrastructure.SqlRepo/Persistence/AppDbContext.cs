using formApi.FormApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace formApi.FormApp.Infrastructure.SqlRepo.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntry> UserEntries => Set<UserEntry>();
    public DbSet<Education> Educations => Set<Education>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntry>()
            .HasMany(u => u.Education)
            .WithOne(e => e.UserEntry)
            .HasForeignKey(e => e.UserEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
