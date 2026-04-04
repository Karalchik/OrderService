using OrderService.Application.DTOs;
using OrderService.Application.Mapping;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.Services;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _repository;
    private readonly TimeProvider _timeProvider;

    public OrderCommandService(IOrderRepository repository, TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<OrderDto> CreateOrderAsync(OrderDto request)
    {
        var order = OrderMapper.ToDomain(request);
        order.Status = OrderStatus.Created;
        order.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        var created = await _repository.CreateAsync(order);
        return OrderMapper.ToDto(created);
    }

    public async Task<OrderDto?> CancelOrderAsync(string id)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order == null) return null;

        if (!order.CanBeCanceled())
            throw new InvalidOperationException($"Order with status '{order.Status}' cannot be cancelled.");

        order.Status = OrderStatus.Canceled;
        var updated = await _repository.UpdateAsync(order);
        return OrderMapper.ToDto(updated);
    }
}