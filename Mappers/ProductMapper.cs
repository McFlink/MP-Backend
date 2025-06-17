using MP_Backend.Models;
using MP_Backend.Models.DTOs.Products;

namespace MP_Backend.Mappers
{
    public static class ProductMapper
    {
        public static ProductDTO ToDetailedDTO(Product product)
        {
            return new ProductDTO
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Variants = product.Variants.Select(v => new ProductVariantDTO
                {
                    Id = v.Id,
                    Scent = v.Scent,
                    Color = v.Color,
                    Size = v.Size,
                    Price = v.Price,
                }).ToList()
            };
        }

        public static List<ProductDTO> ToDetailedDtoList(IEnumerable<Product> products)
        {
            return products.Select(ToDetailedDTO).ToList();
        }

        public static ProductSummaryDTO ToSummaryDTO(Product product)
        {
            return new ProductSummaryDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category
            };
        }

        public static List<ProductSummaryDTO> ToSummaryDTOList(IEnumerable<Product> products)
        {
            return products.Select(ToSummaryDTO).ToList();
        }
    }
}
