using MongoDB.Driver;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure
{
    /// <inheritdoc cref="IOrderRepository"/>
    /// <remarks>Uses MongoDB as the underlying data store with the "OrderDb" database and "Orders" collection.</remarks>
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        /// <summary>
        /// Initializes a new instance of <see cref="OrderRepository"/>.
        /// </summary>
        /// <param name="client">MongoDB client provided via dependency injection.</param>
        public OrderRepository(IMongoClient client)
        {
            var database = client.GetDatabase("OrderDb");
            _orders = database.GetCollection<Order>("Orders");
        }

        /// <inheritdoc/>
        public async Task<Order> CreateAsync(Order order)
        {
            order.Version = 1;
            await _orders.InsertOneAsync(order);
            return order;
        }

        /// <inheritdoc/>
        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        /// <exception cref="ConcurrencyException">
        /// Thrown when the document was modified by another process since it was last read.
        /// </exception>
        public async Task<Order> UpdateAsync(Order order)
        {
            var currentVersion = order.Version;
            order.Version++;

            var result = await _orders.ReplaceOneAsync(
                o => o.Id == order.Id && o.Version == currentVersion,
                order);

            if (result.MatchedCount == 0)
                throw new ConcurrencyException(
                    $"Order '{order.Id}' was modified by another process. " +
                    $"Expected version {currentVersion}, but the document has been updated. Please retry.");

            return order;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> ListAsync(string? userName, string? status, int limit, int offset)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(userName))
                filter &= filterBuilder.Eq(o => o.UserName, userName);

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                filter &= filterBuilder.Eq(o => o.Status, parsedStatus);

            return await _orders.Find(filter)
                                .Skip(offset)
                                .Limit(limit)
                                .SortByDescending(o => o.CreatedAt)
                                .ToListAsync();
        }
    }
}