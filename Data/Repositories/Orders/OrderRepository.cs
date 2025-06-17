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

        // Put in Order model
        public bool OrderConfirmationEmailSent { get; set; } = false;

        public async Task<Order?> GetByOrderIdAsync(Guid orderId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);
        }

        public async Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userId)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                        .AsNoTracking()
                        .ToListAsync(ct);
        }

        public async Task<List<Order>> GetPreviousOrdersSummaryAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userId && OrderConfirmationEmailSent)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<List<Order>> GetPreviousOrdersWithDetailsAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userId && OrderConfirmationEmailSent)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                        .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
