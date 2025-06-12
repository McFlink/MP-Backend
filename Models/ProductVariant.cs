namespace MP_Backend.Models
{
    public class ProductVariant
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string? Scent { get; set; }     // ex: "Kräfta" (för Sticky)
        public string? Color { get; set; }     // ex: "Green Pumpkin" (för gummibete)
        public string? Size { get; set; }      // ex: "14cm"

        public decimal? Price { get; set; }    // Valfritt
    }
}
