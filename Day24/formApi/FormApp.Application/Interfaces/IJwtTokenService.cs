using formApi.FormApp.Domain.Entities;

namespace formApi.FormApp.Application.Interfaces
{
    
        public class GeneratedAccessToken
        {
            public string Token { get; set; } = null!;
            public DateTime ExpiresAt { get; set; }
        }

        public interface IJwtTokenService
        {
            GeneratedAccessToken GenerateAccessToken(AdminUser admin);
            string GenerateRawRefreshToken();//random opaque string 
            string Hash(string rawToken);//ShA-256, used for both storing and looking up
        }

    
}
