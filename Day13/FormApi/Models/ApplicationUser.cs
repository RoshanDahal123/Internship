


using Microsoft.AspNetCore.Identity;
namespace AuthApi.Models;

// Extends Identity's built-in user instead of building one from scratch —
// gives us secure password hashing, lockout, and email uniqueness for free.
public class  ApplicationUser: IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }= string.Empty;
   
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();>
}