using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using AuthApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using AuthApi.Configuration;


namespace AuthApi.Services.Implementations;


public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public EmailService(IOptions<SmtpSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendOtpEmailAsync(string toEmail,string otp)
    {
        if (string.IsNullOrWhiteSpace(_settings.FromAddress))
            throw new InvalidOperationException("Smtp:FromAddress is not configured.");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Your password reset code";
        message.Body = new TextPart("plain")
        {
            Text = $"Your verification code is {otp}. It expires in 5 minutes. " +
                   "If you didn't request this, you can ignore this email."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }


}