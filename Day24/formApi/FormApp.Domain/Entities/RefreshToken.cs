namespace formApi.FormApp.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByTokenHash { get; set; }

        public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;

        public int AdminUserId { get; set; }

        public AdminUser AdminUser { get; set; } = null!;

    }
}
