using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Users;

namespace MP_Backend.Data.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> GetUserProfile(Guid userProfileId, CancellationToken ct)
        {
            var userProfile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userProfileId, ct);

            if (userProfile == null)
                throw new KeyNotFoundException("User profile not found");

            return userProfile;
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile profile, CancellationToken ct)
        {
            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync(ct);

            return profile;
        }
    }
}
