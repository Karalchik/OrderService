using OrderService.Application.DTOs;

namespace OrderService.Application.Services;

public interface IOrderCommandService
{
    Task<OrderDto> CreateOrderAsync(OrderDto request);
    Task<OrderDto?> CancelOrderAsync(string id);
}