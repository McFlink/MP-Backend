using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.ProductVariants
{
    public interface IProductVariantRepository
    {
        Task<List<ProductVariant>> GetByIdAsync(List<Guid> ids, CancellationToken ct);
    }
}
