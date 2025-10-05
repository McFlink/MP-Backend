using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP_Backend.Helpers;
using MP_Backend.Models.DTOs.Products;
using MP_Backend.Services.Products;

namespace MP_Backend.Controllers
{
    [Authorize(Roles = Roles.Retailer)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] bool detailed = false, CancellationToken ct = default)
        {
            if (detailed)
            {
                var detailedProducts = await _productService.GetAllProductsDetailedAsync(ct);
                return Ok(detailedProducts);
            }
            else
            {
                var summaryProducts = await _productService.GetSummariesAsync(ct);
                return Ok(summaryProducts);
            }   
        }
    }
}
