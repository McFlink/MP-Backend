using System.Security.Claims;

namespace MP_Backend.Services.UserServices
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                throw new UnauthorizedAccessException("User ID claim is missing");

            return Guid.Parse(userId);
        }
    }
}
