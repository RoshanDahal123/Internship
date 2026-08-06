

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;
namespace AuthApi.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetOtp> PasswordResetOtps => Set<PasswordResetOtp>();

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>().
            HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
           .HasIndex(rt => rt.TokenHash)
           .IsUnique();

        builder.Entity<PasswordResetOtp>()
          .HasOne(o => o.User)
          .WithMany()
          .HasForeignKey(o => o.UserId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}

    