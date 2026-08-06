

using AuthApi.Models;
namespace AuthApi.Services.Interfaces;
public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    string HashToken(string rawToken);
}