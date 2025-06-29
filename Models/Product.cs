namespace MP_Backend.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
