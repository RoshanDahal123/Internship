

using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs.PasswordReset;

public record VerifyOtpRequestDto
(
    [Required, EmailAddress] string Email,
    [Required, StringLength(6, MinimumLength = 6)] string Otp
 );
