using Microsoft.AspNetCore.Identity;
using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.Users
{
    public interface IUserRepository
    {
        Task<UserProfile> GetUserProfile(Guid userProfileId, CancellationToken ct);
        Task<UserProfile> UpdateUserProfile(UserProfile profile, CancellationToken ct);
        Task<string?> GetLatestCustomerNumberAsync(string prefix, CancellationToken ct);
        Task SoftDeleteUserAsync(IdentityUser user, UserProfile profile, CancellationToken ct);
    }
}
