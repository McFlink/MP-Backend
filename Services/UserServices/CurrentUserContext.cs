using Microsoft.AspNetCore.Identity;
using MP_Backend.Models;

namespace MP_Backend.Services.UserServices
{
    public class CurrentUserContext
    {
        public IdentityUser IdentityUser { get; set; } = null!;
        public UserProfile UserProfile { get; set; } = null!;
    }
}
