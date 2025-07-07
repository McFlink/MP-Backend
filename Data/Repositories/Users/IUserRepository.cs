using Microsoft.AspNetCore.Identity;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;

namespace MP_Backend.Data.Repositories.Users
{
    public interface IUserRepository
    {
        Task<UserProfile> GetUserProfile(Guid userProfileId, CancellationToken ct);
        Task<UserProfile> UpdateUserProfile(UserProfile profile, CancellationToken ct);
    }
}
