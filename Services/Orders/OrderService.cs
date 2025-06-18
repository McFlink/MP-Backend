using MP_Backend.Data.Repositories.Orders;
using MP_Backend.Data.Repositories.Users;
using MP_Backend.Mappers;
using MP_Backend.Models.DTOs.Orders;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IUserContextService userContextService, IUserProfileRepository userprofileRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
                _logger.LogInformation($"Creating new order for user with id {currentUser.IdentityUser.Id}");

                var order = OrderMapper.MapToOrder(dto, currentUser.UserProfile.Id);

                await _orderRepository.CreateOrderAsync(order, ct);
                _logger.LogInformation($"Order created with id: {order.Id}");

                return order.Id;
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
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);

                var orders = await _orderRepository.GetPreviousOrdersSummaryAsync(currentUser.UserProfile.Id, ct);

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
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);

                var orders = await _orderRepository.GetPreviousOrdersWithDetailsAsync(currentUser.UserProfile.Id, ct);
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
