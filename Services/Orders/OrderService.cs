using ClosedXML.Excel;
using MP_Backend.Data.Repositories.Orders;
using MP_Backend.Data.Repositories.ProductVariants;
using MP_Backend.Data.Repositories.Users;
using MP_Backend.Helpers;
using MP_Backend.Mappers;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;
using MP_Backend.Services.Email;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IAppEmailSender _emailSender;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IUserContextService userContextService, IUserProfileRepository userprofileRepository, ILogger<OrderService> logger, IProductVariantRepository productVariantRepository, IAppEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _productVariantRepository = productVariantRepository;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Ordern måste innehålla minst en artikel");

            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);
                if (string.IsNullOrWhiteSpace(currentUser.IdentityUser.Email))
                    throw new InvalidOperationException("Användaren saknar epost-adress");

                _logger.LogInformation($"Creating new order for user with id {currentUser.IdentityUser.Id}");

                // Fetch variantid:s from dto to set current unit price in mapper when creating new OrderItem
                var variantIds = dto.Items.Select(i => i.ProductVariantId).ToList();
                var variants = await _productVariantRepository.GetByIdAsync(variantIds, ct);

                var order = OrderMapper.MapToOrder(dto, currentUser.UserProfile.Id, variants);

                // Generate unique order number
                order.OrderNumber = await GenerateOrderNumberAsync(ct);

                await _orderRepository.CreateOrderAsync(order, ct);
                _logger.LogInformation($"Order created with id: {order.Id} and order number: {order.OrderNumber}");

                await SendOrderConfirmationEmailAsync(order, currentUser.IdentityUser.Email, ct);

                return order.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                throw;
            }
        }

        // Summary for frontend to display only list of IDs and/or order number
        public async Task<OrderSummaryDTO?> GetByOrderIdAsync(Guid orderId, CancellationToken ct)
        {
            try
            {
                var order = await _orderRepository.GetOrderSummaryByIdAsync(orderId, ct);
                if (order is null)
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");

                return OrderMapper.ToSummaryDTO(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching order with ID {orderId}");
                throw;
            }
        }

        public async Task<List<OrderSummaryDTO>> GetPreviousOrdersAsync(CancellationToken ct)
        {
            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);

                var orders = await _orderRepository.GetPreviousOrdersSummaryAsync(currentUser.UserProfile.Id, ct);

                return OrderMapper.ToSummaryDTOList(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching previous orders for current user");
                throw;
            }
        }

        public async Task<List<OrderDetailedDTO>> GetPreviousOrdersWithDetailsAsync(CancellationToken ct)
        {
            try
            {
                var currentUser = await _userContextService.GetCurrentUserWithProfileAsync(ct);

                var orders = await _orderRepository.GetPreviousOrdersWithDetailsAsync(currentUser.UserProfile.Id, ct);
                return OrderMapper.ToDetailedDTOList(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching previous orders with details for current user");
                throw;
            }
        }

        public async Task<byte[]> GenerateOrderHistoryExcelAsync(Guid userProfileId, CancellationToken ct)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userProfileId, ct);
            var userProfile = await _userContextService.GetCurrentUserProfileAsync(ct);
            if (userProfile == null)
                throw new InvalidOperationException("Kunde inte hämta användaren");

            return GenerateOrderHistoryExcel(orders, userProfile, ct);
        }

        private async Task<int> GenerateOrderNumberAsync(CancellationToken ct)
        {
            var latestOrderNumber = await _orderRepository.GetLatestOrderNumberAsync(ct);
            return latestOrderNumber + 1;
        }

        private async Task SendOrderConfirmationEmailAsync(Order order, string toEmail, CancellationToken ct)
        {
            try
            {
                await _emailSender.SendOrderConfimationEmail(order, toEmail, "Order Confirmation", "Thank you for your order!");
                order.OrderConfirmationEmailSent = true;
                await _orderRepository.UpdateAsync(order, ct);
            }
            catch (Exception emailEx)
            {
                _logger.LogError(emailEx, $"Misslyckades att skicka orderbekräftelsemail för order {order.Id}");
            }
        }

        private static Byte[] GenerateOrderHistoryExcel(List<Order> orders, UserProfile userProfile, CancellationToken ct)
        {
            using var workbook = new XLWorkbook();
            // Add new tab (kalkylblad)
            var worksheet = workbook.Worksheets.Add("Order History");

            // Header info block
            worksheet.Cell(1, 1).Value = userProfile.CompanyName ?? ($"{userProfile.FirstName} {userProfile.LastName}");
            worksheet.Cell(2, 1).Value = $"Address: {userProfile.Address}";
            worksheet.Cell(3, 1).Value = $"OrgNo: {userProfile.OrganizationNumber}";
            worksheet.Cell(4, 1).Value = $"Gäller år: {DateTime.UtcNow.Year}"; // Filter...
            worksheet.Cell(5, 1).Value = "* Alla priser är exkl. moms (25%)";

            worksheet.Range("A1:C1").Merge().Style.Font.SetBold().Font.FontSize = 16;
            worksheet.Range("A2:C2").Merge().Style.Font.SetBold();
            worksheet.Range("A3:C3").Merge().Style.Font.SetBold();
            worksheet.Range("A4:C4").Merge().Style.Font.SetBold();
            worksheet.Range("A5:C5").Merge().Style.Font.Italic = true;

            // Headers. (Row, Column)
            worksheet.Cell(7, 1).Value = "Order Number";
            worksheet.Cell(7, 2).Value = "Order Date";
            worksheet.Cell(7, 3).Value = "Items";
            worksheet.Cell(7, 4).Value = "Total Quantity";
            worksheet.Cell(7, 5).Value = "Total Amount (SEK)";
            worksheet.Cell(7, 6).Value = "Total Amount incl. VAT (SEK)";

            worksheet.Range("A7:F7").Style.Font.Bold = true;
            worksheet.Range("A7:F7").Style.Fill.BackgroundColor = XLColor.LightGray;

            var row = 8;
            foreach (var order in orders)
            {
                worksheet.Cell(row, 1).Value = order.OrderNumber.ToString();
                worksheet.Cell(row, 2).Value = order.CreatedAt.ToString("yyyy-MM-dd");

                var itemDescriptions = order.Items
                    .Select(i => $"{i.ProductVariant.Name} x {i.Quantity}")
                    .ToList();
                worksheet.Cell(row, 3).Value = string.Join(", ", itemDescriptions);

                var totalQuantity = order.Items.Sum(i => i.Quantity);
                var totalAmount = order.Items.Sum(i => i.Quantity * i.ProductVariant.Price);
                var totalAmountInclVat = totalAmount * PriceConstants.VatMultiplier;

                worksheet.Cell(row, 4).Value = totalQuantity;
                worksheet.Cell(row, 5).Value = totalAmount;
                worksheet.Cell(row, 6).Value = totalAmountInclVat;

                row++;
            }

            // Totalsumma på sista raden
            worksheet.Cell(row + 1, 4).Value = "Totalt:";
            worksheet.Cell(row + 1, 4).Style.Font.SetBold();
            worksheet.Cell(row + 1, 5).FormulaA1 = $"=SUM(E8:E{row - 1})";
            worksheet.Cell(row + 1, 5).Style.Font.SetBold();
            worksheet.Cell(row + 1, 6).FormulaA1 = $"=SUM(F8:F{row - 1})";
            worksheet.Cell(row + 1, 6).Style.Font.SetBold();

            worksheet.Range($"D{row + 1}:F{row + 1}").Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
