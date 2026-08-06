using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs.Auth;

public record LoginRequestDto(
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password
    );