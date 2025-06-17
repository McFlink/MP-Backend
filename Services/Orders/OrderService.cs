using MP_Backend.Data.Repositories.Orders;
using MP_Backend.Mappers;
using MP_Backend.Models.DTOs.Orders;
using MP_Backend.Services.UserServices;
using System.Security.Claims;

namespace MP_Backend.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IUserContextService userContextService, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public Task<Guid> CreateOrderAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            try
            {
                return null; // Placeholder for actual order creation logic
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                throw;
            }
        }

        public async Task<OrderSummaryDTO?> GetByOrderIdAsync(Guid orderId, CancellationToken ct)
        {
            try
            {
                var order = await _orderRepository.GetByOrderIdAsync(orderId, ct);
                if (order is null)
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");

                return OrderMapper.ToSummaryDTO(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching order with ID {orderId}");
                throw;
            }
        }

        public async Task<List<OrderSummaryDTO>> GetPreviousOrdersAsync(CancellationToken ct)
        {
            try
            {
                var userId = _userContextService.GetCurrentUserId();
                var orders = await _orderRepository.GetPreviousOrdersSummaryAsync(userId, ct);
                return OrderMapper.ToSummaryDTOList(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching previous orders for current user");
                throw;
            }
        }

        public async Task<List<OrderDetailedDTO>> GetPreviousOrdersWithDetailsAsync(CancellationToken ct)
        {
            try
            {
                var userId = _userContextService.GetCurrentUserId();
                var orders = await _orderRepository.GetPreviousOrdersWithDetailsAsync(userId, ct);
                return OrderMapper.ToDetailedDTOList(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching previous orders with details for current user");
                throw;
            }
        }
    }
}
