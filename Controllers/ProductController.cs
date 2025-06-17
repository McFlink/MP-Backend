using Microsoft.AspNetCore.Mvc;
using MP_Backend.Models.DTOs.Products;
using MP_Backend.Services.Products;

namespace MP_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductSummaryDTO>>> GetAll(CancellationToken ct)
        {
            var products = await _productService.GetSummariesAsync(ct);
            return Ok(products);
        }

        [HttpGet("detailed")]
        public async Task<ActionResult<List<ProductDTO>>> GetAllWithVariantsAsync(CancellationToken ct)
        {
            var products = await _productService.GetAllProductsDetailedAsync(ct);
            return Ok(products);
        }
    }
}
