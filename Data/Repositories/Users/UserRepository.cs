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

        public async Task<IdentityUser> UpdateUserEmail(string identityUserId, string newEmail, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(identityUserId, ct);
            if (user == null)
                throw new KeyNotFoundException($"User not found.");

            user.Email = newEmail;
            user.UserName = newEmail;
            user.NormalizedEmail = newEmail.ToUpperInvariant();
            user.NormalizedUserName = newEmail.ToUpperInvariant();

            _context.Users.Update(user);
            await _context.SaveChangesAsync(ct);

            return user;
        }

        public async Task<UserProfile> UpdateUserProfile(Guid userProfileId, UpdateProfileDTO dto, CancellationToken ct)
        {
            var profile = _context.UserProfiles.FirstOrDefault(up => up.Id == userProfileId);
            if (profile == null)
                throw new KeyNotFoundException($"User not found");

            if(!string.IsNullOrEmpty(dto.FirstName))
                profile.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName))
                profile.LastName = dto.LastName;

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                profile.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrEmpty(dto.Address))
                profile.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.BillingAddress))
                profile.BillingAddress = dto.BillingAddress;

            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync(ct);

            return profile;
        }
    }
}
