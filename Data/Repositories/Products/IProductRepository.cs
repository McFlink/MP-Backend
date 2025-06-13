using MP_Backend.Models;

namespace MP_Backend.Data.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllWithVariantsAsync();
    }
}
