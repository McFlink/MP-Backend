namespace MP_Backend.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;        // t.ex. "Gobble Sticky"
        public string Description { get; set; } = null!; // t.ex. "Effektivt doftmedel..."
        public string Category { get; set; } = null!;    // t.ex. "Sticky", "Gummibete"

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
