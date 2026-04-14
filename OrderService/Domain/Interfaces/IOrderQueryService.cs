using OrderService.Application.DTOs;

namespace OrderService.Application.Services;

/// <summary>Defines read (query) operations for the Order aggregate.</summary>
public interface IOrderQueryService
{
    /// <summary>Retrieves a single order by its identifier. Results may be served from cache.</summary>
    /// <param name="id">The order identifier.</param>
    /// <returns>The order DTO.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the order is not found.</exception>
    Task<OrderDto> GetOrderByIdAsync(string id);

    /// <summary>Returns a filtered, paginated list of orders.</summary>
    /// <param name="userName">Filter by user name. Pass <c>null</c> to skip.</param>
    /// <param name="status">Filter by status (case-insensitive). Pass <c>null</c> to skip.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="offset">Number of results to skip for pagination.</param>
    /// <returns>A read-only list of matching order DTOs.</returns>
    Task<IReadOnlyList<OrderDto>> GetOrdersAsync(string? userName, string? status, int limit, int offset);
}