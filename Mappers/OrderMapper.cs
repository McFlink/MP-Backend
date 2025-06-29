using MP_Backend.Models;
using MP_Backend.Models.DTOs.Orders;

namespace MP_Backend.Mappers
{
    public class OrderMapper
    {
        public static OrderDetailedDTO ToDetailedDTO(Order order)
        {
            return new OrderDetailedDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(item => new OrderItemDTO
                {
                    ProductName = item.ProductVariant.Product.Name ?? "Okänd produkt",
                    Name = item.ProductVariant.Name ?? "Okänt namn",
                    Color = item.ProductVariant.Color ?? "N/A",
                    Size = item.ProductVariant.Size ?? "N/A",
                    Quantity = item.Quantity
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

        public static Order MapToOrder(CreateOrderDTO dto, Guid userId, List<ProductVariant> variants)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                UserProfileId = userId,
                CreatedAt = DateTime.UtcNow,
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
