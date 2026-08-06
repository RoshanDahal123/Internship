

using AuthApi.Configuration;
using AuthApi.Data;
using AuthApi.DTOs.Auth;
using AuthApi.DTOs.PasswordReset;
using AuthApi.Models;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly JwtSettings _jwtSettings;
    private readonly OtpSettings _otpSettings;


    // In-memory reset-token store keyed by hash → (userId, expiry).
    // For a single-instance app this is fine; for multi-instance deployments,
    // move this to Redis or the DB (a dedicated ResetTokens table) so all
    // instances see the same state.

    private static readonly Dictionary<string, (string UserId, DateTime Expiry)> ResetTokens = new();
    public AuthService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db,
        ITokenService tokenService,
        IOtpService otpService,
        IEmailService emailService,
        IOptions<JwtSettings> jwtSettings,
        IOptions<OtpSettings> otpSettings)
    {
        _userManager = userManager;
        _db = db;
        _tokenService = tokenService;
        _otpService = otpService;
        _emailService = emailService;
        _jwtSettings = jwtSettings.Value;
        _otpSettings = otpSettings.Value;
    }



    public async Task<(AuthResponseDto user, string accessToken, string refreshToken)> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("An account with this email already exists.");

        }
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,

        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        // ✅ Generate tokens
        //access token:A signed JWT, generated straight from the user object

        var accessToken = _tokenService.GenerateAccessToken(user);
        //2. Refresh token: arandom opaque string (not a JWT)
        var refreshToken = _tokenService.GenerateRefreshToken();

        //store the hashed refresh token in the database
        // 3.We never store the raw refresh token — only its hash — so a DB leak
        //    doesn't hand out valid tokens. This is why HashToken exists.
        var refreshTokenEntity = new RefreshToken
        {
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            UserId = user.Id
        };
        _db.RefreshTokens.Add(refreshTokenEntity);

        await _db.SaveChangesAsync();
        var userDto = new AuthResponseDto(
            user.Id,
             user.Email!,
            user.FirstName,
            user.LastName
            );

        return (userDto, accessToken, refreshToken);
    }
    public async Task<(AuthResponseDto, string, string)> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        // Deliberately vague error — don't reveal whether the email exists.
        // This prevents attackers from using login as an email-enumeration oracle.
        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var (accessToken, refreshToken) = await IssueTokensAsync(user, ip: null);
        return (ToDto(user), accessToken, refreshToken);
    }


    public async Task<(string, string)> RefreshTokenAsync(string rawRefreshToken, string? ip)
    {
        var hash = _tokenService.HashToken(rawRefreshToken);
        var storedToken = await _db.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == hash);

        if (storedToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!storedToken.IsActive)
        {
            // Reuse of a revoked/expired token is a strong signal of theft.
            // Response: revoke every active refresh token for this user,
            // forcing a full re-login everywhere.
            if (storedToken.RevokedAt is not null)
            {
                var allActive = await _db.RefreshTokens
                    .Where(rt => rt.UserId == storedToken.UserId && rt.RevokedAt == null)
                    .ToListAsync();
                foreach (var t in allActive) t.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
            throw new UnauthorizedAccessException("Refresh token is no longer valid.");
        }

        // Rotate: revoke the used token, issue a new one
        storedToken.RevokedAt = DateTime.UtcNow;
        var (accessToken, newRefreshToken) = await IssueTokensAsync(storedToken.User, ip);
        storedToken.ReplacedByTokenHash = _tokenService.HashToken(newRefreshToken);
        await _db.SaveChangesAsync();

        return (accessToken, newRefreshToken);
    }
    public async Task RevokeRefreshTokenAsync(string rawRefreshToken)
    {
        var hash = _tokenService.HashToken(rawRefreshToken);
        var storedToken = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == hash);
        if (storedToken is not null && storedToken.IsActive)
        {
            storedToken.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    public async Task RequestPasswordResetAsync(ForgotPasswordRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        // Same principle as login: don't reveal whether the email exists.
        if (user is null) return;

        var windowStart = DateTime.UtcNow.AddMinutes(-_otpSettings.RequestWindowMinutes);
        var recentCount = await _db.PasswordResetOtps
            .CountAsync(o => o.UserId == user.Id && o.CreatedAt > windowStart);

        if (recentCount >= _otpSettings.MaxRequestsPerWindow)
            throw new InvalidOperationException("Too many reset requests. Please try again later.");

        var latest = await _db.PasswordResetOtps
            .Where(o => o.UserId == user.Id)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (latest is not null &&
            (DateTime.UtcNow - latest.CreatedAt).TotalSeconds < _otpSettings.ResendCooldownSeconds)
        {
            throw new InvalidOperationException("Please wait before requesting another code.");
        }

        var otp = _otpService.GenerateOtp();
        _db.PasswordResetOtps.Add(new PasswordResetOtp
        {
            UserId = user.Id,
            OtpHash = _otpService.HashOtp(otp),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_otpSettings.ExpiryMinutes),
        });
        await _db.SaveChangesAsync();

        await _emailService.SendOtpEmailAsync(user.Email!, otp);
    }


    public async Task<string> VerifyOtpAsync(VerifyOtpRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new InvalidOperationException("Invalid code.");

        var otpRecord = await _db.PasswordResetOtps
            .Where(o => o.UserId == user.Id)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (otpRecord is null || !otpRecord.IsValid)
            throw new InvalidOperationException("Code is invalid or expired.");

        if (!_otpService.VerifyOtp(dto.Otp, otpRecord.OtpHash))
        {
            otpRecord.FailedAttempts++;
            await _db.SaveChangesAsync();
            throw new InvalidOperationException("Incorrect code.");
        }

        otpRecord.IsUsed = true;
        await _db.SaveChangesAsync();

        // Issue a short-lived reset token instead of letting the frontend
        // just proceed to step 3 on its own say-so.
        var resetToken = _tokenService.GenerateRefreshToken();
        var resetTokenHash = _tokenService.HashToken(resetToken);
        ResetTokens[resetTokenHash] = (user.Id, DateTime.UtcNow.AddMinutes(10));

        return resetToken;
    }
    public async Task ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        var hash = _tokenService.HashToken(dto.ResetToken);
        if (!ResetTokens.TryGetValue(hash, out var entry) || entry.Expiry < DateTime.UtcNow)
            throw new InvalidOperationException("Reset session expired. Please request a new code.");

        var user = await _userManager.FindByIdAsync(entry.UserId);
        if (user is null || user.Email != dto.Email)
            throw new InvalidOperationException("Reset session invalid.");

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        ResetTokens.Remove(hash);

        // Revoke every existing refresh token — a password reset should
        // kill all existing sessions, including on other devices.
        var activeTokens = await _db.RefreshTokens
            .Where(rt => rt.UserId == user.Id && rt.RevokedAt == null)
            .ToListAsync();
        foreach (var t in activeTokens) t.RevokedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private async Task<(string accessToken, string refreshToken)> IssueTokensAsync(ApplicationUser user, string? ip)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedByIp = ip,
        });
        await _db.SaveChangesAsync();

        return (accessToken, refreshToken);
    }

    private static AuthResponseDto ToDto(ApplicationUser user) =>
        new(user.Id, user.Email!, user.FirstName, user.LastName);
}
