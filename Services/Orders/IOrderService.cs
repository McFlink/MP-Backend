using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;

namespace MP_Backend.Services.Orders
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetOrdersForCurrentUserAsync(CancellationToken ct);
        Task<OrderDTO?> GetOrderByIdAsync(Guid orderId, CancellationToken ct);
        Task<List<OrderDTO>> GetActiveOrdersForCurrentUserAsync(CancellationToken ct);
        Task<List<OrderDTO>> GetPreviousOrdersForCurrentUserAsync(CancellationToken ct);
        Task<Guid> CreateOrderAsync(CreateOrderDTO dto, CancellationToken ct);
    }
}
