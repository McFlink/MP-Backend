using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<List<Product>> GetAllWithVariantsAsync(CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.Variants)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
