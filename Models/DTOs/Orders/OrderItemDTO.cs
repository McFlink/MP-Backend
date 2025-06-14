namespace MP_Backend.Models.DTOs.Orders
{
    public class OrderItemDTO
    {
        public string ProductName { get; set; } = null!;
        public string Scent { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Weight { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
