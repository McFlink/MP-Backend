using MP_Backend.Data.Repositories.Orders;
using MP_Backend.Mappers;
using MP_Backend.Models.DTOs.Orders;
using System.Security.Claims;

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

        public async Task<List<OrderDetailedDTO>> GetActiveOrdersForCurrentUserAsync(CancellationToken ct)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) throw new UnauthorizedAccessException();

            var guid = Guid.Parse(userId);

            var orders = await _orderRepository.GetByUserIdAsync(guid, ct);

            return OrderMapper.ToDetailedDTOList(orders);
        }

        public Task<OrderDTO?> GetOrderByIdAsync(Guid orderId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetOrdersForCurrentUserAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderSummaryDTO>> GetPreviousOrdersForCurrentUserAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
