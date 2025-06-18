namespace MP_Backend.Models.DTOs.Auth
{
    public class RegisterDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;

        public bool IsRetailer { get; set; }
        public string? OrganizationNumber { get; set; } // If retailer
    }
}
