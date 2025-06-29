using MP_Backend.Migrations;

namespace MP_Backend.Models.DTOs.Orders
{
    public class OrderSummaryDTO
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
