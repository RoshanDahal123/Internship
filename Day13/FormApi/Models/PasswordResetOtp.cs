


namespace AuthApi.Models;

public class PasswordResetOtp
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string OtpHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //wrong attempts counter -locks the otp out after to many bad guesses  
    //independent of expiry. Prevents brute forceing a 6 digit code.
    public int FailedAttempts { get; set; } 
    public bool IsUsed { get; set; }

    public bool IsValid => !IsUsed && FailedAttempts < 5 && DateTime.UtcNow < ExpiresAt;
}