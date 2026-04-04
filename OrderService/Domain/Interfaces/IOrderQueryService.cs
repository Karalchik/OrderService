using OrderService.Application.DTOs;

namespace OrderService.Application.Services;

public interface IOrderQueryService
{
    Task<OrderDto?> GetOrderByIdAsync(string id);
    Task<IReadOnlyList<OrderDto>> GetOrdersAsync(string? userId, string? status, int limit, int offset);
}