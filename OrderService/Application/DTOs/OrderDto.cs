using OrderService.Domain.Models;

namespace OrderService.Application.DTOs;

/// <summary>
/// Data transfer object representing an order in API requests and responses.
/// Used both for creating new orders (POST) and returning order data (GET).
/// </summary>
public record OrderDto
{
    /// <summary>Unique order identifier. <c>null</c> when creating a new order (assigned by the database).</summary>
    public string? Id { get; init; }

    /// <summary>Display name of the user who placed the order.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>Line items included in the order.</summary>
    public List<OrderItemDto> Items { get; init; } = [];

    /// <summary>Current order status. <c>null</c> when creating (automatically set to <see cref="OrderStatus.Created"/>).</summary>
    public OrderStatus? Status { get; init; }

    /// <summary>UTC timestamp of order creation. Populated only in responses.</summary>
    // need to make only in get
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Concurrency version token. Must be included in update requests
    /// to detect conflicts with concurrent modifications.
    /// </summary>
    public int? Version { get; init; }
}