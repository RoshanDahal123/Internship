using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace formApi.FormApp.Infrastructure.Services
{
    public class JwtTokenService:IJwtTokenService
    {

        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public GeneratedAccessToken GenerateAccessToken(AdminUser admin)
        {
            var minutes = int.Parse(_config["Jwt:AccessTokenMinutes"] ?? "15");
            var expires = DateTime.UtcNow.AddMinutes(minutes);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
                new Claim(ClaimTypes.Role, admin.Role), // this is what [Authorize(Roles="Admin")] reads
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret is not configured.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);
            return new GeneratedAccessToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        
        }


        public string GenerateRawRefreshToken()
        {
           var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);   
        }


        public string Hash(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToBase64String(bytes);
        }



    }
}
