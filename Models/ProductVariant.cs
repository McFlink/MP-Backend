namespace MP_Backend.Models
{
    public class ProductVariant
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }

        public decimal? Price { get; set; }    // Should retailers see price?
    }
}
