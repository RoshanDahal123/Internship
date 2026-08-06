


using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs.Auth;

public record RegisterRequestDto(
    [Required, MinLength(2)] string FirstName,
      [Required, MinLength(2)] string LastName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password
    );
