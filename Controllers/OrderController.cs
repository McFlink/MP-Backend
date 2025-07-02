using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP_Backend.Helpers;
using MP_Backend.Models.DTOs.Orders;
using MP_Backend.Services.Orders;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Controllers
{
    [Authorize(Roles = Roles.Retailer)]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserContextService _userContextService;

        public OrderController(IOrderService orderService, IUserContextService userContextService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto, CancellationToken ct)
        {
            var orderId = await _orderService.CreateOrderAsync(dto, ct);
            return CreatedAtAction(nameof(GetOrderById), new { orderId }, null);
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken ct)
        {
            var order = await _orderService.GetByOrderIdAsync(orderId, ct);
            return Ok(order);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetPreviousOrdersSummary(CancellationToken ct)
        {
            var orders = await _orderService.GetPreviousOrdersAsync(ct);
            return Ok(orders);
        }

        [HttpGet("detailed")]
        public async Task<IActionResult> GetPreviousOrdersWithDetails(CancellationToken ct)
        {
            var orders = await _orderService.GetPreviousOrdersWithDetailsAsync(ct);
            return Ok(orders);
        }

        [HttpGet("download-excel")]
        public async Task<IActionResult> DownloadOrderHistory(CancellationToken ct)
        {
            var currentUserId = await _userContextService.GetCurrentUserProfileIdAsync(ct);
            var fileBytes = await _orderService.GenerateOrderHistoryExcelAsync(currentUserId, ct);

            var fileName = $"order-history-mp-fishing-supply-ab-{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
