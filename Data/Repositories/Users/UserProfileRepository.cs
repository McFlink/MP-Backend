using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.Users
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByUserIdAsync(string identityUserId, CancellationToken ct)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == identityUserId, ct);
        }
    }
}
