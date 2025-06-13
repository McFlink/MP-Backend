using MP_Backend.Data.Repositories.Products;
using MP_Backend.Mappers;
using MP_Backend.Models.DTOs;

namespace MP_Backend.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<ProductDTO>> GetAllProductsDetailedAsync(CancellationToken ct)
        {
            try
            {
                var products = await _productRepository.GetAllWithVariantsAsync(ct);
                return ProductMapper.ToDetailedDtoList(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching products including variants");
                throw;
            }
        }

        public async Task<List<ProductSummaryDTO>> GetSummariesAsync(CancellationToken ct)
        {
            try
            {
                var products = await _productRepository.GetAllAsync(ct);
                return ProductMapper.ToSummaryDTOList(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching products");
                throw;
            }
        }
    }
}
