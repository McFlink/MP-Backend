using MP_Backend.Models.DTOs;

namespace MP_Backend.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsDetailedAsync(CancellationToken ct);
        Task<List<ProductSummaryDTO>> GetSummariesAsync(CancellationToken ct);
    }
}
