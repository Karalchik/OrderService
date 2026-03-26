using MongoDB.Driver;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("OrderDb");
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task CreateAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

        //HRL
        public async Task<IEnumerable<Order>> ListAsync(string? userId, string? status, int limit, int offset)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(userId))
                filter &= filterBuilder.Eq(o => o.UserId, userId);

            if (!string.IsNullOrEmpty(status))
                filter &= filterBuilder.Eq(o => o.Status, status);

            return await _orders.Find(filter)
                                .Skip(offset)
                                .Limit(limit)
                                .SortByDescending(o => o.CreatedAt)
                                .ToListAsync();
        }
    }
}