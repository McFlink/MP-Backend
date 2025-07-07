using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MP_Backend.Models.DTOs.Auth;
using MP_Backend.Services.Auth;

namespace MP_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IAuthService authService, UserManager<IdentityUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok("Registrering lyckades");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            await _authService.LoginAsync(dto);
            return Ok("Inloggad");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(Response);
            return NoContent();
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Felaktig användare");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? Ok("E-post bekräftad!") : BadRequest("Bekräftelsen misslyckades.");
        }

        // Temporary for debug/testing
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = _authService.GetCurrentUserId();
            return Ok(new { UserId = userId });
        }
    }
}
