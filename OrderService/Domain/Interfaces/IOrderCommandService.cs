using OrderService.Application.DTOs;

namespace OrderService.Application.Services;

/// <summary>Write operations for orders.</summary>
public interface IOrderCommandService
{
    /// <summary>Creates a new order.</summary>
    Task<OrderDto> CreateOrderAsync(OrderDto request);

    /// <summary>Cancels an order.</summary>
    /// <exception cref="InvalidOperationException">Order cannot be canceled.</exception>
    Task<OrderDto?> CancelOrderAsync(string id);
}