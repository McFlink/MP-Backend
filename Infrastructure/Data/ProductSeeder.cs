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
                Description = "Effektivt doftmedel för sportfiske",
                Category = "Sticky",
                Variants = new List<ProductVariant>
                {
                    new() { Scent = "Kräfta" },
                    new() { Scent = "Räka" },
                    new() { Scent = "Mussla" },
                    new() { Scent = "Fisk" }
                }
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();
        }
    }
}
