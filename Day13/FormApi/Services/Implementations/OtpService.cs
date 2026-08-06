using System.Security.Cryptography;
using AuthApi.Configuration;
using AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
namespace AuthApi.Services.Implementations;


public class  OtpService: IOtpService
{
    
    private readonly OtpSettings _otpSettings;
    public OtpService(IOptions<OtpSettings> otpSettings)
    {
        _otpSettings = otpSettings.Value;
    }

    public string GenerateOtp()
    {
        // Numeric OTP using a cryptographically secure RNG — Random.Shared
        // is NOT safe for this, it's predictable.

        var max = (int)Math.Pow(10, _otpSettings.Length);
        var otp = RandomNumberGenerator.GetInt32(0, max);
        return otp.ToString().PadLeft(_otpSettings.Length, '0');
    }

    public string HashOtp(string rawOtp)
    {
        var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawOtp));
        return Convert.ToHexString(bytes);
    }

    public bool VerifyOtp(string rawOtp, string storedHash)
    {

        var computedHash = HashOtp(rawOtp);
        // Constant-time comparison — prevents timing attacks that could
        // otherwise leak the correct OTP one character at a time.
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromHexString(computedHash),
            Convert.FromHexString(storedHash));
    }

}