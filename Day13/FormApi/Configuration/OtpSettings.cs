

namespace AuthApi.Configuration;

public class OtpSettings
{
    public const string SectionName = "Otp";
    public int Length { get; set; } = 6;
    public int ExpiryMinutes { get; set; } = 10;
    public int MaxFailedAttempts { get; set; } = 5;
    public int ResendCooldownSeconds { get; set; } = 60;
    // Hard cap on OTP requests per email within the rolling window below —
    // stops someone from spamming a user's inbox or hammering your email provider
    public int MaxRequestsPerWindow { get; set; } = 5;
    public int RequestWindowMinutes { get; set; } = 60;
}