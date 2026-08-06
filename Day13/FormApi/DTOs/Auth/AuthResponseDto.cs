namespace AuthApi.DTOs.Auth;


public record AuthResponseDto(
    string UserId,
    string Email,
    string FirstName,
    string LastName
);