namespace MP_Backend.Models.DTOs.Orders
{
    public class CreateOrderDTO
    {
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}
