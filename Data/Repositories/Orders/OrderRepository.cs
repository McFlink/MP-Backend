using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;

namespace MP_Backend.Data.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public async Task <List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userId)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
