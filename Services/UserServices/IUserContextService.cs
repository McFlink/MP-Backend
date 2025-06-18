using MP_Backend.Models;

namespace MP_Backend.Services.UserServices
{
    public interface IUserContextService
    {
        string GetCurrentIdentityUserId();
        Task<Guid> GetCurrentUserProfileIdAsync(CancellationToken ct);
        Task<UserProfile?> GetCurrentUserProfileAsync(CancellationToken ct);
        Task<CurrentUserContext> GetCurrentUserWithProfileAsync(CancellationToken ct);
    }
}
