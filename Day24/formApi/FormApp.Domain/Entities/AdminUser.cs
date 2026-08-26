using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace formApi.FormApp.Domain.Entities
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = "Admin"; // Default role is Admin

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
 }

