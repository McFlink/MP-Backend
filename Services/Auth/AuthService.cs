using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MP_Backend.Models.DTOs;
using System.Security.Claims;
using MP_Backend.Controllers;
using MP_Backend.Services.Email;

namespace MP_Backend.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<(bool Success, string? Error)> LoginAsync(LoginDTO dto);
        string? GetCurrentUserId();
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;
        private readonly IAppEmailSender _emailSender;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService,
            IAppEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
            _emailSender = emailSender;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"https://localhost:7067/api/auth/confirmemail?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(user.Email, "Verifiera ditt konto", confirmationLink);

            return true;
        }

        public async Task<(bool Success, string? Error)> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return (false, "E-posten hittas inte"); // 

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return (false, "Du måste bekräfta din e-post först.");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return (false, "Fel lösenord");

            var token = _jwtService.GenerateToken(user.Id, user.Email!);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return (true, null);
        }

        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
