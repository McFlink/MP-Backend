using Microsoft.AspNetCore.Identity;
using MP_Backend.Data.Repositories.Users;
using MP_Backend.Models;
using System.Security.Claims;

namespace MP_Backend.Services.UserServices
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public UserContextService(IHttpContextAccessor httpContextAccessor, IUserProfileRepository userProfileRepository, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userProfileRepository = userProfileRepository;
            _userManager = userManager;
        }

        public string GetCurrentIdentityUserId()
        {
            var identityUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (identityUserId is null)
                throw new UnauthorizedAccessException("User ID claim is missing");

            return identityUserId;
        }

        public async Task<Guid> GetCurrentUserProfileIdAsync(CancellationToken ct)
        {
            var identityUserId = GetCurrentIdentityUserId();
            var userProfile = await _userProfileRepository.GetByUserIdAsync(identityUserId, ct);
            if (userProfile is null)
                throw new UnauthorizedAccessException("User profile not found");

            return userProfile.Id;
        }

        public async Task<UserProfile?> GetCurrentUserProfileAsync(CancellationToken ct)
        {
            var identityUserId = GetCurrentIdentityUserId();
            var profile = await _userProfileRepository.GetByUserIdAsync(identityUserId, ct);

            if (profile is null)
                throw new UnauthorizedAccessException("User profile not found");

            return profile;
        }

        public async Task<CurrentUserContext> GetCurrentUserWithProfileAsync(CancellationToken ct)
        {
            var identityUserId = GetCurrentIdentityUserId();

            var identityUser = await _userManager.FindByIdAsync(identityUserId);
            if (identityUser == null)
                throw new UnauthorizedAccessException("Identity user not found");

            var userProfile = await _userProfileRepository.GetByUserIdAsync(identityUserId, ct);
            if (userProfile == null)
                throw new UnauthorizedAccessException("User profile not found");

            return new CurrentUserContext
            {
                IdentityUser = identityUser,
                UserProfile = userProfile
            };
        }

    }
}
