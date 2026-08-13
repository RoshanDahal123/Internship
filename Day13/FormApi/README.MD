
# Folder Structure for AuthApi
```` AuthApi/
├── Controllers/
│   └── AuthController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── Auth/
│   │   ├── RegisterRequestDto.cs
│   │   ├── LoginRequestDto.cs
│   │   └── AuthResponseDto.cs
│   └── PasswordReset/
│       ├── ForgotPasswordRequestDto.cs
│       ├── VerifyOtpRequestDto.cs
│       └── ResetPasswordRequestDto.cs
├── Models/
│   ├── ApplicationUser.cs
│   ├── RefreshToken.cs
│   └── PasswordResetOtp.cs
├── Services/
│   ├── Interfaces/
│   │   ├── ITokenService.cs
│   │   ├── IAuthService.cs
│   │   ├── IOtpService.cs
│   │   └── IEmailService.cs
│   └── Implementations/
│       ├── TokenService.cs
│       ├── AuthService.cs
│       ├── OtpService.cs
│       └── EmailService.cs
├── Configuration/
│   ├── JwtSettings.cs
│   └── OtpSettings.cs
├── Program.cs
└── appsettings.json
````

## Nutget packages
````
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package MailKit
````

Here in the modal section ApplicationUser.cs , we have defined the properties for the user model
and it inherits from IdentityUser class which is provided by Microsoft.AspNetCore.Identity.EntityFrameworkCore package.
This class contains all the properties required for user authentication and authorization.
No need to manually hash the password, encrypt the password , make the password secure, and
 validate the password because IdentityUser class already provides these functionalities.
 Lockout and email uniqueness for free.