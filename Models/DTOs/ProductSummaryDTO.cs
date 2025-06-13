namespace MP_Backend.Models.DTOs
{
    public class ProductSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
