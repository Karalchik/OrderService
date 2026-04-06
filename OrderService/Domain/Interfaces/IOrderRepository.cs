using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces
{
    /// <summary>Persistence contract for the <see cref="Order"/> aggregate.</summary>
    public interface IOrderRepository
    {
        /// <summary>Persists a new order.</summary>
        Task<Order> CreateAsync(Order order);

        /// <summary>Replaces an existing order with updated data.</summary>
        Task<Order> UpdateAsync(Order order);

        /// <summary>Returns the order with the given <paramref name="id"/>, or <c>null</c>.</summary>
        Task<Order?> GetByIdAsync(string id);

        /// <summary>Returns a filtered, paginated list of orders sorted by date descending.</summary>
        Task<IEnumerable<Order>> ListAsync(string? userId, string? status, int limit, int offset);
    }
}