using MP_Backend.Models.DTOs.Users;
using System.Runtime.CompilerServices;

namespace MP_Backend.Services.UserServices
{
    public interface IUserService
    {
        Task<UserProfileDTO> GetUserProfileAsync(CancellationToken ct);
        Task UpdateEmailAsync(string newEmail, CancellationToken ct);
        Task<UserProfileDTO> UpdateProfileAsync(UpdateProfileDTO dto, CancellationToken ct);
        Task<string> GenerateCustomerNumberAsync(bool isRetailer, CancellationToken ct);
        Task SoftDeleteAccountAsync(CancellationToken ct);
    }
}
