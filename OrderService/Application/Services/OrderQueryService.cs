using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using OrderService.Application.DTOs;
using OrderService.Application.Mapping;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Services;

/// <inheritdoc cref="IOrderQueryService"/>
public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<OrderQueryService> _logger;

    public OrderQueryService(IOrderRepository repository, IDistributedCache cache, ILogger<OrderQueryService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <remarks>Uses Redis cache with 5-minute expiration (cache-aside pattern).</remarks>
    public async Task<OrderDto?> GetOrderByIdAsync(string id)
    {
        string cacheKey = $"order_{id}";

        var cachedOrder = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedOrder))
        {
            _logger.LogInformation("Order {OrderId} found in cache", id);
            return JsonSerializer.Deserialize<OrderDto>(cachedOrder);
        }

        _logger.LogInformation("Order {OrderId} not found in cache. Fetching from MongoDB", id);
        var order = await _repository.GetByIdAsync(id);

        if (order is null) return null;

        var dto = OrderMapper.ToDto(order);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto), cacheOptions);

        return dto;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(string? userName, string? status, int limit, int offset)
    {
        var orders = await _repository.ListAsync(userName, status, limit, offset);
        return orders.Select(OrderMapper.ToDto).ToList();
    }
}