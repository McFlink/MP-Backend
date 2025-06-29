using Microsoft.EntityFrameworkCore;
using MP_Backend.Data;
using MP_Backend.Models;

namespace MP_Backend.Infrastructure.Data
{
    public class ProductSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (await context.Products.AnyAsync())
                return; // Redan seedat

            var product = new Product
            {
                Name = "Gobble Sticky",
                Description = "Effektivt doftmedel för all sorts fiske.",
                Category = "Doftmedel",
                Variants = new List<ProductVariant>
                {
                    new() { Name = "Kräfta" },
                    new() { Name = "Räka" },
                    new() { Name = "Mussla" },
                    new() { Name = "Fisk" }
                }
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();
        }
    }
}
