using MP_Backend.Models.DTOs;

namespace MP_Backend.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
    }
}
