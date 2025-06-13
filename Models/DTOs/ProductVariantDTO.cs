namespace MP_Backend.Models.DTOs
{
    public class ProductVariantDTO
    {
        public Guid VariantId { get; set; }
        public string? Scent { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Price { get; set; }
    }
}
