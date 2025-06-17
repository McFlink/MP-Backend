using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;

namespace MP_Backend.Data.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    }
}
