using Microsoft.AspNetCore.Identity;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;

namespace MP_Backend.Data.Repositories.Users
{
    public interface IUserRepository
    {
        Task<IdentityUser> UpdateUserEmail(string identityUserId, string newEmail, CancellationToken ct);
        Task<UserProfile> UpdateUserProfile(Guid identityUserId, UpdateProfileDTO dto, CancellationToken ct);
    }
}
