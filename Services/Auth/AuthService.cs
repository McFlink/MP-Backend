using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MP_Backend.Controllers;
using MP_Backend.Services.Email;
using MP_Backend.Models.DTOs.Auth;
using MP_Backend.Data;
using MP_Backend.Models;

namespace MP_Backend.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<(bool Success, string? Error)> LoginAsync(LoginDTO dto);
        Task LogoutAsync(HttpResponse response);
        string? GetCurrentUserId();
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;
        private readonly IAppEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService,
            IAppEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return false;

            var role = dto.IsRetailer ? "Retailer" : "Customer";
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded) return false;

            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                BillingAddress = dto.IsRetailer ? dto.BillingAddress : null,
                CreatedAt = DateTime.UtcNow,
                OrganizationNumber = dto.IsRetailer ? dto.OrganizationNumber : null,
                BankIdVerified = false,
                BankIdVerifiedAt = null
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://localhost:7067/api/auth/confirmemail?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await _emailSender.SendEmailAsync(user.Email, "Verifiera ditt konto", confirmationLink);

            return true;
        }

        public async Task<(bool Success, string? Error)> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return (false, "E-posten hittas inte");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return (false, "Du måste bekräfta din e-post först.");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return (false, "Fel lösenord");

            var token = await _jwtService.GenerateToken(user);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return (true, null);
        }

        public Task LogoutAsync(HttpResponse response)
        {
            response.Cookies.Delete("jwt");
            return Task.CompletedTask;
        }


        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
