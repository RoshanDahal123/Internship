using formApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace formApi.Data

{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

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

}
