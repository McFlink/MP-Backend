using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MP_Backend.Data;
using MP_Backend.Mappers;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Auth;
using MP_Backend.Services.Email;
using MP_Backend.Services.UserServices;
using System.Security.Claims;

namespace MP_Backend.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto, CancellationToken ct);
        Task LoginAsync(LoginDTO dto);
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
        private readonly ILogger<AuthService> _logger;
        private readonly IUserService _userService;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService,
            IAppEmailSender emailSender,
            ApplicationDbContext context,
            ILogger<AuthService> logger,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
            _emailSender = emailSender;
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        public async Task RegisterAsync(RegisterDTO dto, CancellationToken ct)
        {
            IdentityUser? user = null;

            try
            {
                var customerNumber = await _userService.GenerateCustomerNumberAsync(dto.IsRetailer, ct);

                user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Registrering av användare misslyckades: {errors}");
                }

                var role = dto.IsRetailer ? "Retailer" : "Customer";
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    // Delete user if role assignment fails
                    await _userManager.DeleteAsync(user);

                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    _logger.LogWarning($"Failed to assign role {role}: {errors}");
                    throw new InvalidOperationException($"Roll-tilldelning misslyckades: {errors}. Ny användare har ej skapats.");
                }

                var userProfile = UserMapper.ToUserProfile(user, dto, customerNumber);

                _context.UserProfiles.Add(userProfile);

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UQ_CustomerNumber") == true)
                {
                    // Double protection - remove user to prevent "hanging" users
                    await _userManager.DeleteAsync(user);
                    _logger.LogError(ex, "Duplicerat kundnummer");
                    throw new InvalidOperationException("Registrering misslyckades: Ett fel uppstod, försök igen.");
                }

                try
                {
                    // Send verification email to newly registered user
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = $"https://localhost:7067/api/auth/confirmemail?userId={user.Id}&token={Uri.EscapeDataString(token)}";
                    await _emailSender.SendEmailAsync(user.Email, "Verifiera ditt konto", confirmationLink);
                }
                catch (Exception mailEx)
                {
                    _logger.LogWarning(mailEx, "Verifieringsmail kunde inte skickas. Användaren är skapad men oaktiverad.");
                }
            }
            catch
            {
                if (user?.Id != null)
                {
                    var existingUser = await _userManager.FindByIdAsync(user.Id);
                    if (existingUser != null)
                        await _userManager.DeleteAsync(user);
                }
            }
        }

        public async Task LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Felaktiga inloggningsuppgifter");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedAccessException("Ditt konto är låst."); // NÄSTA STEG I BRANCH: Kolla så att radering av userprofile inte raderar dess orders.

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new InvalidOperationException("Du måste bekräfta din e-post först.");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    // Send mail to user here. "Your account has min locked due to too many failed login attemps".
                    _logger.LogWarning($"Konto låst: {dto.Email}");
                }

                throw new UnauthorizedAccessException("Felaktiga inloggningsuppgifter");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var token = await _jwtService.GenerateToken(user);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
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
