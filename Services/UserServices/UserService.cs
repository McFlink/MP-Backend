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

        public UserService(IUserRepository userRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
        }

        public async Task<IdentityUser> UpdateEmailAsync(string newEmail, CancellationToken ct)
        {
            var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
            if (currentUser == null)
                throw new InvalidOperationException("Current user is not authenticated.");

            return await _userRepository.UpdateUserEmail(currentUser.IdentityUser.Id, newEmail, ct);
        }

        public async Task<UserProfileDTO> UpdateProfileAsync(ProfileUpdateDTO dto, CancellationToken ct)
        {
            var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
            if (currentUser == null)
                throw new InvalidOperationException("Current user is not authenticated.");

            var updatedProfile = await _userRepository.UpdateUserProfile(currentUser.UserProfile.Id, dto, ct);

            return UserMapper.ToUserProfileDTO(updatedProfile);
        }
    }
}
