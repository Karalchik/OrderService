using OrderService.Application.DTOs;
using OrderService.Application.Mapping;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Services;

/// <inheritdoc cref="IOrderQueryService"/>
public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderRepository _repository;

    public OrderQueryService(IOrderRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public async Task<OrderDto?> GetOrderByIdAsync(string id)
    {
        var order = await _repository.GetByIdAsync(id);
        return order is null ? null : OrderMapper.ToDto(order);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(
        string? userId, string? status, int limit, int offset)
    {
        var orders = await _repository.ListAsync(userId, status, limit, offset);
        return orders.Select(OrderMapper.ToDto).ToList();
    }
}