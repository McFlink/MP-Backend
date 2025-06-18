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

        public async Task<Order> CreateOrderAsync(Order order, CancellationToken ct)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(ct);
            return order;
        }

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

        public async Task<List<Order>> GetByUserIdAsync(Guid userProfileId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userProfileId)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                        .AsNoTracking()
                        .ToListAsync(ct);
        }

        public async Task<List<Order>> GetPreviousOrdersSummaryAsync(Guid userProfileId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userProfileId && o.OrderConfirmationEmailSent)
                .AsNoTracking() 
                .ToListAsync(ct);
        }

        public async Task<List<Order>> GetPreviousOrdersWithDetailsAsync(Guid userProfileId, CancellationToken ct)
        {
            return await _context.Orders
                .Where(o => o.UserProfileId == userProfileId && o.OrderConfirmationEmailSent)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                        .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
