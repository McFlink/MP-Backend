namespace MP_Backend.Models.DTOs.Orders
{
    public class OrderItemDTO
    {
        public string ProductName { get; set; } = null!;
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        //public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
