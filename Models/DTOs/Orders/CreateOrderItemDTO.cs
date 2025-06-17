namespace MP_Backend.Models.DTOs.Orders
{
    public class CreateOrderItemDTO
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
