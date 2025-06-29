using System.ComponentModel.DataAnnotations;

namespace MP_Backend.Models.DTOs.Auth
{
    public class RegisterDTO : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;

        //Contact person
        [Required]
        public string FirstName { get; set; } = default!;
        [Required]
        public string LastName { get; set; } = default!;

        [Required]
        public string CompanyName { get; set; } = default!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = default!;
        [Required]
        public string Address { get; set; } = default!;
        public string BillingAddress { get; set; } = default!;

        public bool IsRetailer { get; set; }
        public string? OrganizationNumber { get; set; } // If retailer

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsRetailer && string.IsNullOrWhiteSpace(OrganizationNumber))
            {
                yield return new ValidationResult(
                    "OrganizationNumber is required when registering as a retailer.",
                    new[] { nameof(OrganizationNumber) });
            }
        }
    }
}
