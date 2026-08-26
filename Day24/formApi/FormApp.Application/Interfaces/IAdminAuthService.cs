
using formApi.FormApp.Application.DTOs.Auth;
namespace formApi.FormApp.Application.Interfaces
{
    public interface IAdminAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterAdminDto dto);
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<AuthResultDto> RefreshAsync(string rawRefreshToken);
        Task LogoutAsync (string rawRefreshToken);
    }
}
