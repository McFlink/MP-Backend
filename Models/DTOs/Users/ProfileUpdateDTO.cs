namespace MP_Backend.Models.DTOs.Users
{
    public class ProfileUpdateDTO
    {
        // First- and lastname for retailer is their Contact person
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? BillingAddress { get; set; }
    }
}
