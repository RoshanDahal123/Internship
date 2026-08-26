namespace formApi.FormApp.Application.DTOs.Auth
{
    public class RegisterAdminDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string SetupKey { get; set; } = null!;//temporary
    }
}
