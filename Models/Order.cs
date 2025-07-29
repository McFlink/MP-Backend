namespace MP_Backend.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public int OrderNumber { get; set; }
        public Guid? UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; } // Nullable only to be able to use global query filter
        public string? CustomerNameAtOrder { get; set; }
        public string? CustomerEmailAtOrder { get; set; }
        public string? OrganizationNumberAtOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool OrderConfirmationEmailSent { get; set; } = false;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
