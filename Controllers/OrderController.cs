using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP_Backend.Infrastructure.Identity;
using MP_Backend.Models.DTOs.Orders;
using MP_Backend.Services.Orders;
using System.Runtime.CompilerServices;

namespace MP_Backend.Controllers
{
    [Authorize(Roles = Roles.Retailer)]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto, CancellationToken ct)
        {
            if (!dto.Items.Any())
                return BadRequest();

            var orderId = await _orderService.CreateOrderAsync(dto, ct);
            return CreatedAtAction(nameof(GetOrderById), new { orderId }, null);
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken ct)
        {
            try
            {
                var order = await _orderService.GetByOrderIdAsync(orderId, ct);
                if (order is null)
                    return NotFound(new { message = $"Order with ID {orderId} not found." });
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetPreviousOrdersSummary(CancellationToken ct)
        {
            try
            {
                var orders = await _orderService.GetPreviousOrdersAsync(ct);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("detailed")]
        public async Task<IActionResult> GetPreviousOrdersWithDetails(CancellationToken ct)
        {
            try
            {
                var orders = await _orderService.GetPreviousOrdersWithDetailsAsync(ct);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
