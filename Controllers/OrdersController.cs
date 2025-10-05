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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserContextService _userContextService;

        public OrdersController(IOrderService orderService, IUserContextService userContextService)
        {
            _orderService = orderService;
            _userContextService = userContextService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderSummaryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto, CancellationToken ct)
        {
            var orderId = await _orderService.CreateOrderAsync(dto, ct);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderSummaryDTO>> GetOrderById(Guid id, CancellationToken ct)
        {
            var order = await _orderService.GetByOrderIdAsync(id, ct);
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] bool detailed = false, CancellationToken ct = default)
        {
            if (detailed)
            {
                var detailedOrders = await _orderService.GetPreviousOrdersWithDetailsAsync(ct);
                return Ok(detailedOrders);
            }
            else
            {
                var summaryOrders = await _orderService.GetPreviousOrdersAsync(ct);
                return Ok(summaryOrders);
            }
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> DownloadOrderHistory(CancellationToken ct)
        {
            var currentUserId = await _userContextService.GetCurrentUserProfileIdAsync(ct);
            var fileBytes = await _orderService.GenerateOrderHistoryExcelAsync(currentUserId, ct);

            var fileName = $"order-history-mp-fishing-supply-ab-{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
