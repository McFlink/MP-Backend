using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;

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
                OrganizationNumber = userProfile.OrganizationNumber
            };
        }
    }
}
