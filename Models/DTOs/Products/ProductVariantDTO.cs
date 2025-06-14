namespace MP_Backend.Models.DTOs.Products
{
    public class ProductVariantDTO
    {
        public Guid Id { get; set; }
        public string? Scent { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Price { get; set; }
    }
}
