using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data
{
    public class EcommerceDataContext:DbContext 
    {
        public EcommerceDataContext(DbContextOptions<EcommerceDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole>UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new {ur.RoleId,ur.UserId});
            
            modelBuilder.Entity<UserRole>()
                .HasOne(my=>my.User)
                .WithMany(u=>u.UserRoles)
                .HasForeignKey(my=> my.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(my => my.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(my => my.RoleId);
        }


    }
}
