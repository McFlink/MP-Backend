using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken ct);
        Task<List<Product>> GetAllWithVariantsAsync(CancellationToken ct);
    }
}
