using Microsoft.AspNetCore.Mvc;
using MP_Backend.Services.Orders;
using System.Runtime.CompilerServices;

namespace MP_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto, CancellationToken ct)
        //{
        //    try
        //    {
        //        var orderId = await _orderService.CreateOrderAsync(dto, ct);
        //        return CreatedAtAction(nameof(GetByOrderId), new { orderId }, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

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
