using OrderService.Application.DTOs;
using OrderService.Application.Mapping;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.Services;

/// <inheritdoc cref="IOrderCommandService"/>
public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _repository;
    private readonly TimeProvider _timeProvider;
    private readonly OrderMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="OrderCommandService"/>.
    /// </summary>
    /// <param name="repository">Order persistence repository.</param>
    /// <param name="timeProvider">Provider used to generate UTC timestamps for new orders.</param>
    /// <param name="mapper">Mapperly-based mapper for Order/DTO conversions.</param>
    public OrderCommandService(IOrderRepository repository, TimeProvider timeProvider, OrderMapper mapper)
    {
        _repository = repository;
        _timeProvider = timeProvider;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<OrderDto> CreateOrderAsync(OrderDto request)
    {
        var order = _mapper.ToDomain(request);
        order.Status = OrderStatus.Created;
        order.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        var created = await _repository.CreateAsync(order);
        return _mapper.ToDto(created);
    }

    /// <inheritdoc/>
    public async Task<OrderDto> CancelOrderAsync(string id)
    {
        var order = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Order with id '{id}' was not found.");

        if (!order.CanBeCanceled())
            throw new InvalidOperationException($"Order with status '{order.Status}' cannot be cancelled.");

        order.Status = OrderStatus.Canceled;
        var updated = await _repository.UpdateAsync(order);
        return _mapper.ToDto(updated);
    }
}