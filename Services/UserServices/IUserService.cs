using Microsoft.AspNetCore.Identity;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;

namespace MP_Backend.Services.UserServices
{
    public interface IUserService
    {
        Task<IdentityUser> UpdateEmailAsync(string newEmail, CancellationToken ct);
        Task<UserProfileDTO> UpdateProfileAsync(ProfileUpdateDTO dto, CancellationToken ct);
    }
}
