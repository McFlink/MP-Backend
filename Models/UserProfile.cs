using Microsoft.AspNetCore.Identity;

namespace MP_Backend.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRetailer { get; set; }

        public string? NationalId { get; set; } // For individuals (BankId)
        public string? OrganizationNumber { get; set; } // For organizations

        public bool BankIdVerified { get; set; }
        public DateTime? BankIdVerifiedAt { get; set; }
    }
}
