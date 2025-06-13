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

        public async Task<List<Product>> GetAllWithVariantsAsync()
        {
            return await _context.Products
                .Include(p => p.Variants)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
