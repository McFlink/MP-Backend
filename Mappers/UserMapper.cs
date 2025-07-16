using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;
using MP_Backend.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace MP_Backend.Mappers
{
    public class UserMapper
    {
        public static UserProfileDTO ToUserProfileDTO(UserProfile userProfile)
        {
            if (userProfile == null) throw new ArgumentNullException(nameof(userProfile));

            return new UserProfileDTO
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PhoneNumber = userProfile.PhoneNumber,
                Address = userProfile.Address,
                BillingAddress = userProfile.BillingAddress,
                OrganizationNumber = userProfile.OrganizationNumber,
                CompanyName = userProfile.CompanyName
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
    }
}
