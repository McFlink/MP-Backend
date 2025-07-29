using MP_Backend.Helpers;
using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;
using MP_Backend.Services.UserServices;

namespace MP_Backend.Mappers
{
    public class OrderMapper
    {
        public static OrderDetailedDTO ToDetailedDTO(Order order)
        {
            const decimal shippingFee = PriceConstants.ShippingFee;
            const decimal vatRate = PriceConstants.VatRate;

            var itemNetTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);
            var netTotalWithShipping = itemNetTotal + shippingFee;
            var vatAmount = netTotalWithShipping * vatRate;
            var totalAmount = netTotalWithShipping + vatAmount;

            return new OrderDetailedDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.CreatedAt,
                TotalNetAmount = netTotalWithShipping,
                VatAmount = vatAmount,
                TotalAmount = totalAmount,
                ShippingFee = shippingFee,
                Items = order.Items.Select(item => new OrderItemDTO
                {
                    ProductName = item.ProductVariant.Product.Name ?? "Okänd produkt",
                    Name = item.ProductVariant.Name ?? "Okänt namn",
                    Color = item.ProductVariant.Color ?? "N/A",
                    Size = item.ProductVariant.Size ?? "N/A",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    LineTotal = item.UnitPrice * item.Quantity
                }).ToList()
            };
        }

        public static List<OrderDetailedDTO> ToDetailedDTOList(IEnumerable<Order> orders)
        {
            return orders.Select(ToDetailedDTO).ToList();
        }

        public static OrderSummaryDTO ToSummaryDTO(Order order)
        {
            return new OrderSummaryDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.CreatedAt
            };
        }

        public static List<OrderSummaryDTO> ToSummaryDTOList(IEnumerable<Order> orders)
        {
            return orders.Select(ToSummaryDTO).ToList();
        }

        public static Order MapToOrder(CreateOrderDTO dto, CurrentUserContext user, List<ProductVariant> variants)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                UserProfileId = user.UserProfile.Id,
                CreatedAt = DateTime.UtcNow,
                CustomerNameAtOrder = $"{user.UserProfile.FirstName} {user.UserProfile.LastName}",
                CustomerEmailAtOrder = user.IdentityUser.Email,
                OrganizationNumberAtOrder = user.UserProfile.OrganizationNumber,
                Items = dto.Items.Select(item =>
                {
                    var variant = variants.First(v => v.Id == item.ProductVariantId);
                    return new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductVariantId = item.ProductVariantId,
                        Quantity = item.Quantity,
                        UnitPrice = variant.Price ?? 0
                    };
                }).ToList()
            };
        }
    }
}
