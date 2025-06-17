using MP_Backend.Models.DTOs.Products;

namespace MP_Backend.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsDetailedAsync(CancellationToken ct);
        Task<List<ProductSummaryDTO>> GetSummariesAsync(CancellationToken ct);
    }
}
