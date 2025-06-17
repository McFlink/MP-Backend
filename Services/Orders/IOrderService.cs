using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;

namespace MP_Backend.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderSummaryDTO?> GetByOrderIdAsync(Guid orderId, CancellationToken ct);
        Task<List<OrderSummaryDTO>> GetPreviousOrdersAsync(CancellationToken ct);
        Task<List<OrderDetailedDTO>> GetPreviousOrdersWithDetailsAsync(CancellationToken ct);
        Task<Guid> CreateOrderAsync(CreateOrderDTO dto, CancellationToken ct);
    }
}
