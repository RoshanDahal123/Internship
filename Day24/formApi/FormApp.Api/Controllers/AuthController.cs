using formApi.FormApp.Application.DTOs.Auth;
using formApi.FormApp.Application.Exceptions;
using formApi.FormApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace formApi.FormApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminAuthService _authService;

        public AuthController(IAdminAuthService authService)
        {
            _authService = authService;
        }

       

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterAdminDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                if (result is null) return Unauthorized();
                SetAuthCookies(result);
                return Ok(new AuthResponseDto
                {
                    Role= result.Role,
                    Email = result.Email,
                });
            }
            catch (AuthException ex) { return Unauthorized(new { message = ex.Message }); }
            catch (AppValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }


        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);

                SetAuthCookies(result);

                // Return non-sensitive info only — don't put tokens in the body anymore
                return Ok(new AuthResponseDto
                {
                    Role = result.Role,
                    Email = result.Email,
                });
            }
            catch (AuthException ex) { return Unauthorized(new { message = ex.Message }); }
            catch (AppValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]

        public async Task<IActionResult> Refresh()
        {
            
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Unauthorized(new { message = "No refresh token." });

            try
            {
                var result = await _authService.RefreshAsync(refreshToken);
                SetAuthCookies(result);
                return Ok(new AuthResponseDto
                {
                    Role = result.Role,
                    Email = result.Email,
                });
            }
            catch (AuthException ex) {
                ClearAuthCookies();
                return Unauthorized(new { message = ex.Message }); 
            }
            catch (AppValidationException ex) { return Unauthorized(new { message = ex.Message }); }
            
            

        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "No refresh token." });
            try
            {
                await _authService.LogoutAsync(refreshToken);
                ClearAuthCookies();
                return NoContent();
                
            }
            catch (AuthException ex) { return Unauthorized(new { message = ex.Message }); }
        }


        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (email is null)
                return Unauthorized();

            return Ok(new AuthResponseDto
            {
                Role = role,
                Email=email,
            });
        }
        private void SetAuthCookies(AuthResultDto result)
        {
            Response.Cookies.Append("accessToken", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,          // requires HTTPS — fine for you since Kestrel dev cert is https
                SameSite = SameSiteMode.None, // or Lax if frontend/backend are on different subdomains
                Expires = result.AccessTokenExpiresAt
            });

            Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = result.RefreshTokenExpiresAt,
                Path = "/api/auth" // optional: restrict refresh token cookie to only be sent to this route
            });
        }

        private void ClearAuthCookies()
        {
            Response.Cookies.Delete("accessToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/" // match whatever path accessToken was set with (default "/")
            });

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api/auth" // MUST match the Path used in SetAuthCookies
            });
        }

    }
 }


