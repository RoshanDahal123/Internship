using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs.PasswordReset;

// ResetToken is the short-lived token issued by VerifyOtp — NOT the raw OTP.
// This is the fix for the bypass risk I flagged on the frontend: the server
// re-validates a token it issued, rather than trusting the client's step state.
public record ResetPasswordRequestDto(
    [Required, EmailAddress] string Email,
    [Required] string ResetToken,
    [Required, MinLength(8)] string NewPassword
);