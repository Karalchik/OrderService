using MongoDB.Driver;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure
{
    /// <inheritdoc cref="IOrderRepository"/>
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IMongoClient client)
        {
            var database = client.GetDatabase("OrderDb");
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
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