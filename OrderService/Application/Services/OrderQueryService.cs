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
    private readonly OrderMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="OrderQueryService"/>.
    /// </summary>
    /// <param name="repository">Order persistence repository.</param>
    /// <param name="cache">Redis distributed cache for order lookups.</param>
    /// <param name="logger">Logger for cache hit/miss diagnostics.</param>
    /// <param name="mapper">Mapperly-based mapper for Order/DTO conversions.</param>
    public OrderQueryService(IOrderRepository repository, IDistributedCache cache, ILogger<OrderQueryService> logger, OrderMapper mapper)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Implements a cache-aside pattern using Redis with a 5-minute absolute expiration.
    /// On cache miss the order is fetched from MongoDB and stored in cache for subsequent reads.
    /// </remarks>
    public async Task<OrderDto> GetOrderByIdAsync(string id)
    {
        string cacheKey = $"order_{id}";

        var cachedOrder = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrWhiteSpace(cachedOrder))
        {
            _logger.LogInformation("Order {OrderId} found in cache", id);
            return JsonSerializer.Deserialize<OrderDto>(cachedOrder)!;
        }

        _logger.LogInformation("Order {OrderId} not found in cache. Fetching from MongoDB", id);
        var order = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Order with id '{id}' was not found.");

        var dto = _mapper.ToDto(order);

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
        return orders.Select(o => _mapper.ToDto(o)).ToList();
    }
}