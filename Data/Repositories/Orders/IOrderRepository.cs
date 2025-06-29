using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;
using System.Runtime.CompilerServices;

namespace MP_Backend.Data.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task<Order?> GetByOrderIdAsync(Guid orderId, CancellationToken ct);
        Task<List<Order>> GetPreviousOrdersSummaryAsync(Guid userId, CancellationToken ct);
        Task<List<Order>> GetPreviousOrdersWithDetailsAsync(Guid userId, CancellationToken ct);
        Task<Order> CreateOrderAsync(Order order, CancellationToken ct);
        Task<int> GetLatestOrderNumberAsync(CancellationToken ct);
    }
}
