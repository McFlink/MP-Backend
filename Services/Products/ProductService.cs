using MP_Backend.Data.Repositories.Products;
using MP_Backend.Models.DTOs;

namespace MP_Backend.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllWithVariantsAsync();

                return products.Select(p => new ProductDTO
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Category = p.Category,
                    Variants = p.Variants.Select(v => new ProductVariantDTO
                    {
                        VariantId = v.Id,
                        Scent = v.Scent,
                        Color = v.Color,
                        Size = v.Size,
                        Price = v.Price,
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                // Implement logging
                throw new Exception("Error when fetching all products with variants", ex);
            }
        }
    }
}
