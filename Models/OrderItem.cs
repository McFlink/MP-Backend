namespace MP_Backend.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
