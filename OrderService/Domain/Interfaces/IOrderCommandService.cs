using OrderService.Application.DTOs;

namespace OrderService.Application.Services;

/// <summary>Defines write (command) operations for the Order aggregate.</summary>
public interface IOrderCommandService
{
    /// <summary>Creates a new order with status <see cref="OrderStatus.Created"/>.</summary>
    /// <param name="request">DTO containing user name and line items.</param>
    /// <returns>The created order DTO with generated Id, status, and timestamp.</returns>
    Task<OrderDto> CreateOrderAsync(OrderDto request);

    /// <summary>Cancels an existing order by transitioning its status to <see cref="OrderStatus.Canceled"/>.</summary>
    /// <param name="id">The order identifier.</param>
    /// <returns>The canceled order DTO.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the order is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the order has already been delivered or canceled.</exception>
    Task<OrderDto> CancelOrderAsync(string id);
}