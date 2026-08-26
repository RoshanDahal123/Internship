namespace formApi.FormApp.Application.DTOs.Auth
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; } = null!;
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiresAt { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;

    }
}
