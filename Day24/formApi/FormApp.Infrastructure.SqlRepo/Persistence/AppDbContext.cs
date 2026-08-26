using formApi.FormApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace formApi.FormApp.Infrastructure.SqlRepo.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntry> UserEntries => Set<UserEntry>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminUser>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(t => t.AdminUser)
            .HasForeignKey(t => t.AdminUserId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<UserEntry>()
            .HasMany(u => u.Education)
            .WithOne(e => e.UserEntry)
            .HasForeignKey(e => e.UserEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
