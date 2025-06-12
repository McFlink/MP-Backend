using MP_Backend.Enums;

namespace MP_Backend.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; } = null!;

        public DateTime BookingDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public bool BankIdVerified { get; set; }
        public DateTime? BankIdVerifiedAt { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? PaidAt { get; set; }
        public string? PaymentMethod { get; set; } // t ex "Swish"
        public string? PaymentReference { get; set; } // Swish orderId eller liknande

        public string LegalAgreementVersion { get; set; } = "v1.0";
        public DateTime AgreementAcceptedAt { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    }
}
