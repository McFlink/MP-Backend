namespace MP_Backend.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
