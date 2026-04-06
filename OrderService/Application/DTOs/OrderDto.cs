using OrderService.Domain.Models;

namespace OrderService.Application.DTOs;

/// <summary>DTO for order API requests and responses.</summary>
public record OrderDto
{
    public string? Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public List<OrderItemDto> Items { get; init; } = [];
    public OrderStatus? Status { get; init; }
    public DateTime? CreatedAt { get; init; }
}