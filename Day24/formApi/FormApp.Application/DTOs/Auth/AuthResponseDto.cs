namespace formApi.FormApp.Application.DTOs.Auth
{
    public class AuthResponseDto
    {

        public string AccessToken { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime AccessTokenExpiresAt { get; set; } = null!;
    }
    }
}
