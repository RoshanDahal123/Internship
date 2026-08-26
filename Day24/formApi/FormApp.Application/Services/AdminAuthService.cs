using Azure.Core;

using formApi.FormApp.Application.DTOs.Auth;

 using formApi.FormApp.Application.Exceptions;
using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Domain.Entities;

using System.Runtime.InteropServices;

namespace formApi.FormApp.Application.Services
{
    public class AdminAuthService:IAdminAuthService
    {
        private readonly IAdminUserRepository _adminRepo;

        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwt;
        private readonly IConfiguration _config;

        public AdminAuthService(
            IAdminUserRepository adminRepo,
            IRefreshTokenRepository refreshRepo,
            IPasswordHasher hasher,
            IJwtTokenService jwt,
            IConfiguration config)
        {
            _adminRepo = adminRepo;
            _refreshRepo = refreshRepo;
            _hasher = hasher;
            _jwt = jwt;
            _config = config;
        }
        public async Task<AuthResultDto> RegisterAsync(RegisterAdminDto dto)
        {
            var expectedKey = _config["Auth:SetupKey"];
            if (string.IsNullOrWhiteSpace(expectedKey) || dto.SetupKey != expectedKey)
                throw new AuthException("Invalid setup key.");
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new AuthException("Email and password are required");
            }
            if (dto.Password.Length < 8)
                throw new AppValidationException("Password must be at least 8 characters.");

            if(await _adminRepo.EmailExistsAsync(dto.Email))
                throw new AppValidationException("Email already exists.");

            var admin = new AdminUser
            {
                Email = dto.Email.Trim().ToLowerInvariant(),
                PasswordHash= _hasher.Hash(dto.Password),
                Role="Admin"
            };

            await _adminRepo.AddAsync(admin);

            return await IssueTokensAsync(admin);
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
          var admin= await _adminRepo.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());
            if(admin is null || !_hasher.Verify(dto.Password, admin.PasswordHash))
                throw new AuthException("Invalid email or password.");
            return await IssueTokensAsync(admin);
        }

        public async Task<AuthResultDto> RefreshAsync(string rawRefreshToken)
        {
            var hash = _jwt.Hash(rawRefreshToken);
            var existing = await _refreshRepo.GetByTokenHashAsync(hash);

            if (existing is null || !existing.IsActive)
                throw new AuthException("Invalid or expired refresh token.");

            // rotate: revoke the old one, issue a new one
            existing.RevokedAt = DateTime.UtcNow;

            var result = await IssueTokensAsync(existing.AdminUser);

            existing.ReplacedByTokenHash = _jwt.Hash(result.RefreshToken);
            await _refreshRepo.SaveChangesAsync();

            return result;
        }


        public async Task LogoutAsync(string rawRefreshToken)
        {
            var hash = _jwt.Hash(rawRefreshToken);
            var existing = await _refreshRepo.GetByTokenHashAsync(hash);

            if(existing is not null && existing.IsActive
                )
            {
                existing.RevokedAt = DateTime.UtcNow;
                await _refreshRepo.SaveChangesAsync();
            }
        }
        private async Task<AuthResultDto> IssueTokensAsync(AdminUser admin)
        {
            var access = _jwt.GenerateAccessToken(admin);
            var rawRefresh = _jwt.GenerateRawRefreshToken();
            var refreshDays= int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7");
            var refreshExpiresAt = DateTime.UtcNow.AddDays(refreshDays);

            await _refreshRepo.AddAsync(new RefreshToken
            {
                AdminUserId = admin.Id,
                TokenHash = _jwt.Hash(rawRefresh),
                ExpiresAt = refreshExpiresAt
            });
            await _refreshRepo.SaveChangesAsync();

            return new AuthResultDto  
            {
                AccessToken = access.Token,
                AccessTokenExpiresAt = access.ExpiresAt,
                RefreshToken = rawRefresh,
                RefreshTokenExpiresAt = refreshExpiresAt,
                Email = admin.Email,
                Role = admin.Role
            };

        }
        

        


    }
}
