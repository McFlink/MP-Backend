namespace MP_Backend.Models.DTOs.Users
{
    public class UserProfileDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string BillingAddress { get; set; } = null!;
        public string? OrganizationNumber { get; set; }
        public string? CompanyName { get; set; }
    }
}
