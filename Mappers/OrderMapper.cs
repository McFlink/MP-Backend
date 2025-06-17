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
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(item => new OrderItemDTO
                {
                    ProductName = item.ProductVariant.Product.Name ?? "Okänd produkt",
                    Scent = item.ProductVariant.Scent ?? "Okänd färg",
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
                CreatedAt = order.CreatedAt
            };
        }

        public static List<OrderSummaryDTO> ToSummaryDTOList(IEnumerable<Order> orders)
        {
            return orders.Select(ToSummaryDTO).ToList();
        }
    }
}
