using Microsoft.AspNetCore.Identity;
using MP_Backend.Data.Repositories.Users;
using MP_Backend.Mappers;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;

namespace MP_Backend.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IUserContextService userContextService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(CancellationToken ct)
        {
            try
            {
                var currentUserProfileId = await _userContextService.GetCurrentUserProfileIdAsync(ct);
                if (currentUserProfileId == Guid.Empty)
                    throw new InvalidOperationException("Current user is not authenticated.");

                var profile = await _userRepository.GetUserProfile(currentUserProfileId, ct);

                return UserMapper.ToUserProfileDTO(profile);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user profile.");
                throw;
            }
        }

        public async Task<IdentityUser> UpdateEmailAsync(string newEmail, CancellationToken ct)
        {
            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
                if (currentUser == null)
                    throw new InvalidOperationException("Current user is not authenticated.");

                return await _userRepository.UpdateUserEmail(currentUser.IdentityUser.Id, newEmail, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user email");
                throw;
            }
        }

        public async Task<UserProfileDTO> UpdateProfileAsync(UpdateProfileDTO dto, CancellationToken ct)
        {
            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
                if (currentUser == null)
                    throw new InvalidOperationException("Current user is not authenticated.");

                var updatedProfile = await _userRepository.UpdateUserProfile(currentUser.UserProfile.Id, dto, ct);

                return UserMapper.ToUserProfileDTO(updatedProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user profile");
                throw;
            }
        }
    }
}
