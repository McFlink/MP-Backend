using MP_Backend.Data.Repositories.Orders;
using MP_Backend.Models.DTOs.Orders;

namespace MP_Backend.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
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
