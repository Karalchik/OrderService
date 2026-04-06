using OrderService.Application.DTOs;

namespace OrderService.Application.Services;

/// <summary>Read operations for orders.</summary>
public interface IOrderQueryService
{
    /// <summary>Returns the order with the given <paramref name="id"/>, or <c>null</c>.</summary>
    Task<OrderDto?> GetOrderByIdAsync(string id);

    /// <summary>Returns a filtered, paginated list of orders.</summary>
    Task<IReadOnlyList<OrderDto>> GetOrdersAsync(string? userId, string? status, int limit, int offset);
}