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

        public Task<Guid> CreateOrderAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetActiveOrdersForCurrentUserAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO?> GetOrderByIdAsync(Guid orderId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetOrdersForCurrentUserAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetPreviousOrdersForCurrentUserAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
