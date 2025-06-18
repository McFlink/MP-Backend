using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.Users
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(string identityUserId, CancellationToken ct);
    }
}
