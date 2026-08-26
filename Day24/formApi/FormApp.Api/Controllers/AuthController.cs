using formApi.FormApp.Application.DTOs.Auth;
using formApi.FormApp.Application.Exceptions;
using formApi.FormApp.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

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
                return Ok(result);
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
                return Ok(result);
            }
            catch (AuthException ex) { return Unauthorized(new { message = ex.Message }); }
            catch (AppValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]

        public async Task<IActionResult> Refresh(RefreshRequestDto dto)
        {
            try
            {
                var result = await _authService.RefreshAsync(dto.RefreshToken);
                return Ok(result);
            }
            catch (AuthException ex) { return Unauthorized(new { message = ex.Message }); }

        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequestDto dto)
        {
            try
            {
                await _authService.LogoutAsync(dto.RefreshToken);
                return NoContent();
            }
            catch (AuthException ex) { return Unauthorized(new { message = ex.Message }); }
        }

    }
 }


