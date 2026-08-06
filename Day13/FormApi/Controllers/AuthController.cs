using AuthApi.DTOs.Auth;
using AuthApi.DTOs.PasswordReset;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        try
        {
            var (user, accessToken, refreshToken) = await _authService.RegisterAsync(dto);
            SetAuthCookies(accessToken, refreshToken);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
            var (user, accessToken, refreshToken) = await _authService.LoginAsync(dto);
            SetAuthCookies(accessToken, refreshToken);
            return Ok(user);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var rawRefreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(rawRefreshToken))
            return Unauthorized(new { message = "No refresh token provided." });

        try
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var (accessToken, newRefreshToken) = await _authService.RefreshTokenAsync(rawRefreshToken, ip);
            SetAuthCookies(accessToken, newRefreshToken);
            return Ok(new { message = "Token refreshed" });
        }
        catch (UnauthorizedAccessException ex)
        {
            ClearAuthCookies();
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var rawRefreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(rawRefreshToken))
            await _authService.RevokeRefreshTokenAsync(rawRefreshToken);

        ClearAuthCookies();
        return Ok(new { message = "Logged out" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto dto)
    {
        try
        {
            await _authService.RequestPasswordResetAsync(dto);
        }
        catch (InvalidOperationException ex)
        {
            // Rate-limit errors are fine to surface; existence of the email is not
            return BadRequest(new { message = ex.Message });
        }
        // Always 200 here for non-rate-limit cases — don't leak whether the email exists
        return Ok(new { message = "If that email is registered, a code has been sent." });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequestDto dto)
    {
        try
        {
            var resetToken = await _authService.VerifyOtpAsync(dto);
            return Ok(new { resetToken });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto dto)
    {
        try
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new { message = "Password updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private void SetAuthCookies(string accessToken, string refreshToken)
    {
        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,           // HTTPS only — set false only for local http dev if absolutely needed
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15),
            Path = "/",
        };
        Response.Cookies.Append("accessToken", accessToken, accessCookieOptions);

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            // Scoped to the refresh endpoint only — the browser won't even
            // send this cookie on ordinary API calls, shrinking its exposure.
            Path = "/api/auth/refresh-token",
        };
        Response.Cookies.Append("refreshToken", refreshToken, refreshCookieOptions);
    }

    private void ClearAuthCookies()
    {
        Response.Cookies.Delete("accessToken", new CookieOptions { Path = "/" });
        Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/auth/refresh-token" });
    }
}