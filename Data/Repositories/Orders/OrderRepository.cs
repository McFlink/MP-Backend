using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;

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
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                 .FirstOrDefaultAsync(ct);
        }
        public async Task<Order?> GetOrderSummaryByIdAsync(Guid orderId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync(ct);
        }


        public async Task<List<Order>> GetByUserIdAsync(Guid userProfileId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                    .Where(o => o.UserProfileId == userProfileId)
                    .Include(o => o.Items)
                        .ThenInclude(oi => oi.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                    .ToListAsync(ct);
        }

        public async Task<List<Order>> GetPreviousOrdersSummaryAsync(Guid userProfileId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.UserProfileId == userProfileId && o.OrderConfirmationEmailSent)
                .ToListAsync(ct);
        }

        public async Task<List<Order>> GetPreviousOrdersWithDetailsAsync(Guid userProfileId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.UserProfileId == userProfileId && o.OrderConfirmationEmailSent)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .ToListAsync(ct);
        }

        public async Task<int> GetLatestOrderNumberAsync(CancellationToken ct)
        {
            return await _context.Orders.MaxAsync(o => (int?)o.OrderNumber, ct) ?? 1000; // Check highest orderNo, allow null. If null, start at 1000
        }

        public async Task UpdateAsync(Order order, CancellationToken ct)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(ct);
        }

    }
}
