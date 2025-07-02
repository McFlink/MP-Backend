namespace MP_Backend.Models.DTOs.Orders
{
    public class OrderDetailedDTO
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalNetAmount { get; set; }           // exkl moms, inkl frakt
        public decimal ShippingFee { get; set; }              // 99 kr
        public decimal VatAmount { get; set; }                // 25 % av nettopriset
        public decimal TotalAmount { get; set; }              // inkl moms och frakt

        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
