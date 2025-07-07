using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using MP_Backend.Data.Repositories.Users;
using MP_Backend.Mappers;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;
using System.ComponentModel.DataAnnotations;

namespace MP_Backend.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IUserContextService userContextService, ILogger<UserService> logger, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _userManager = userManager;
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

        public async Task UpdateEmailAsync(string newEmail, CancellationToken ct)
        {
            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
                if (currentUser == null)
                    throw new InvalidOperationException("Current user is not authenticated.");

                ValidateEmailFormat(newEmail);

                var existingUser = await _userManager.FindByEmailAsync(newEmail);
                if (existingUser == null && existingUser.Id != currentUser.IdentityUser.Id)
                    throw new InvalidOperationException("E-posten används redan");

                var user = currentUser.IdentityUser;

                user.Email = newEmail;
                user.UserName = newEmail;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Kunde inte uppdatera e-post: {errors}");
                }
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

                if (dto.IsRetailer && string.IsNullOrWhiteSpace(dto.BillingAddress))
                    throw new ArgumentException("Faktureringsadress krävs för återförsäljare");

                var profile = currentUser.UserProfile;

                profile.FirstName = dto.FirstName ?? currentUser.UserProfile.FirstName;
                profile.LastName = dto.LastName ?? currentUser.UserProfile.LastName;
                profile.PhoneNumber = dto.PhoneNumber ?? currentUser.UserProfile.PhoneNumber;
                profile.Address = dto.Address ?? currentUser.UserProfile.Address;
                currentUser.UserProfile.BillingAddress = dto.BillingAddress ?? profile.BillingAddress;

                var updatedProfile = await _userRepository.UpdateUserProfile(profile, ct);

                return UserMapper.ToUserProfileDTO(updatedProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user profile");
                throw;
            }
        }

        private void ValidateEmailFormat(string email)
        {
            if (!new EmailAddressAttribute().IsValid(email))
                throw new ArgumentException("Ogiltigt e-postformat.");
        }

    }
}
