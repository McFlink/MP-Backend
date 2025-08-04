using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;
using MP_Backend.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Mappers
{
    public class UserMapper
    {
        public static UserProfileDTO ToUserProfileDTO(CurrentUserContext user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return new UserProfileDTO
            {
                FirstName = user.UserProfile.FirstName,
                LastName = user.UserProfile.LastName,
                Email = user.IdentityUser.Email!,
                PhoneNumber = user.UserProfile.PhoneNumber,
                Address = user.UserProfile.Address,
                BillingAddress = user.UserProfile.BillingAddress,
                IsRetailer = user.UserProfile.IsRetailer,
                OrganizationNumber = user.UserProfile.OrganizationNumber,
                CompanyName = user.UserProfile.CompanyName
            };
        }

        public static UserProfile ToUserProfile(IdentityUser user, RegisterDTO dto, string customerNumber)
        {
            return new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CustomerNumber = customerNumber,
                IsRetailer = dto.IsRetailer,
                PhoneNumber = dto.PhoneNumber,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                BillingAddress = dto.IsRetailer ? dto.BillingAddress : null,
                CreatedAt = DateTime.UtcNow,
                OrganizationNumber = dto.IsRetailer ? dto.OrganizationNumber : null,
                BankIdVerified = false,
                BankIdVerifiedAt = null
            };
        }

        public static AuthUserDTO ToAuthUserDTO(UserProfile profile, IdentityUser user)
        {
            return new AuthUserDTO()
            {
                FirstName = profile.FirstName,
                CompanyName = profile.CompanyName,
                IsRetailer = profile.IsRetailer,
                Email = user.Email!
            };
        }
    }
}
