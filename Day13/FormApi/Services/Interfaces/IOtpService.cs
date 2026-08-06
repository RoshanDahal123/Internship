namespace AuthApi.Services.Interfaces;

public interface IOtpService
{
    string GenerateOtp();
    string HashOtp(string rawOtp);
    bool VerifyOtp(string rawOtp, string storedHash);
}