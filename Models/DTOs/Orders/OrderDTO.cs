namespace MP_Backend.Models.DTOs.Orders
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
