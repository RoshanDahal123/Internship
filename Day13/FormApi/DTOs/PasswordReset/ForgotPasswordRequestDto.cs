using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs.PasswordReset;

public record ForgotPasswordRequestDto([Required, EmailAddress] string Email);