namespace MP_Backend.Models.DTOs.Auth
{
    public class AuthUserDTO
    {
        public string FirstName { get; set; } = default!;
        public string? CompanyName { get; set; }
        public bool IsRetailer { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
