using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.ProductVariants
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductVariantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductVariant>> GetByIdAsync(List<Guid> ids, CancellationToken ct)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Product)
                .Where(v => ids.Contains(v.Id))
                .ToListAsync(ct);
        }
    }
}
