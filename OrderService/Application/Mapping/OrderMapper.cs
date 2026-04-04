using OrderService.Application.DTOs;
using OrderService.Domain.Models;


namespace OrderService.Application.Mapping;

public static class OrderMapper
{
    public static OrderDto ToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserName = order.UserName,
            Items = order.Items?.Select(ToDto).ToList() ?? [],
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };
    }

    public static OrderItemDto ToDto(OrderItem item)
    {
        return new OrderItemDto
        {
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            Price = item.Price
        };
    }

    public static Order ToDomain(OrderDto dto)
    {
        return new Order
        {
            Id = dto.Id ?? string.Empty,
            UserName = dto.UserName,
            Status = dto.Status ?? OrderStatus.Created,
            Items = dto.Items?.Select(ToDomain).ToList() ?? []
        };
    }

    public static OrderItem ToDomain(OrderItemDto dto)
    {
        return new OrderItem
        {
            ProductName = dto.ProductName,
            Quantity = dto.Quantity,
            Price = dto.Price
        };
    }
}