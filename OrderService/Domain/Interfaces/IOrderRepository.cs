using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<Order?> GetByIdAsync(string id);
        Task<IEnumerable<Order>> ListAsync(string? userId, string? status, int limit, int offset);
    }
}