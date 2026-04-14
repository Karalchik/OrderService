using OrderService.Application.DTOs;
using OrderService.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace OrderService.Application.Mapping;

/// <summary>
/// Mapperly-based mapper between <see cref="Order"/> domain entities and <see cref="OrderDto"/> DTOs.
/// Compile-time source generator — zero runtime reflection overhead.
/// </summary>
[Mapper]
public partial class OrderMapper
{
    /// <summary>Maps an <see cref="Order"/> domain entity to an <see cref="OrderDto"/>.</summary>
    public partial OrderDto ToDto(Order order);

    /// <summary>Maps an <see cref="OrderItem"/> to an <see cref="OrderItemDto"/>.</summary>
    public partial OrderItemDto ItemToDto(OrderItem item);

    /// <summary>Maps an <see cref="OrderItemDto"/> to an <see cref="OrderItem"/> domain entity.</summary>
    public partial OrderItem ItemToDomain(OrderItemDto dto);

    /// <summary>
    /// Maps an <see cref="OrderDto"/> to an <see cref="Order"/> domain entity.
    /// Applies defaults: empty Id and <see cref="OrderStatus.Created"/> when not provided.
    /// </summary>
    public Order ToDomain(OrderDto dto)
    {
        return new Order
        {
            Id = dto.Id ?? string.Empty,
            UserName = dto.UserName,
            Status = dto.Status ?? OrderStatus.Created,
            Items = dto.Items.Select(ItemToDomain).ToList(),
            Version = dto.Version ?? 1
        };
    }
}