


using AuthApi.DTOs.Auth;
using AuthApi.DTOs.PasswordReset;

namespace AuthApi.Services.Interfaces;

public interface IAuthService {
Task<(AuthResponseDto user,string accessToken, string refreshToken)> RegisterAsync(RegisterRequestDto registerRequest);
    Task<(AuthResponseDto user, string accessToken, string refreshToken)> LoginAsync(LoginRequestDto dto);
    Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string rawRefreshToken, string? ip);
    Task RevokeRefreshTokenAsync(string rawRefreshToken);
    Task RequestPasswordResetAsync(ForgotPasswordRequestDto dto);
    Task<string> VerifyOtpAsync(VerifyOtpRequestDto dto); //returns short-lived reset token
    Task ResetPasswordAsync(ResetPasswordRequestDto dto);

}
