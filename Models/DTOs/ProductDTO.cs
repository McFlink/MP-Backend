namespace MP_Backend.Models.DTOs
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public List<ProductVariantDTO> Variants { get; set; } = new();
    }
}
