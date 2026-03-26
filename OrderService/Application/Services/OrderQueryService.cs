using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Application.Services
{
    public class OrderQueryService
    {
        private readonly IOrderRepository _repository;

        public OrderQueryService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string? userId, string? status, int limit, int offset)
        {
            //можно написать сюда кеш
            return await _repository.ListAsync(userId, status, limit, offset);
        }
    }
}