
namespace AuthApi.Models;

public class RefreshToken
{
    public int Id { get; set; }

    // We store a HASH of the token, never the raw value — same principle as passwords.
    // If the DB leaks, an attacker can't use these to authenticate.
    public string TokenHash { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null;
    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public string? CreatedByIp { get; set; }

    //Points to the token that raplaced this one- lets us trace a rotation chain and 
    //detect reyse if an already-rotated(i.e.stolen) token
    public string? ReplacedByTokenHash { get; set; }
    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;



}