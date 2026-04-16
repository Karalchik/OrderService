namespace OrderService.Application.DTOs;

/// <summary>
/// Request DTO for creating a new order.
/// Contains only the fields that the client should provide.
/// </summary>
public record CreateOrderRequest
{
    /// <summary>Display name of the user who placed the order.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>Line items included in the order.</summary>
    public List<OrderItemDto> Items { get; init; } = [];
}
