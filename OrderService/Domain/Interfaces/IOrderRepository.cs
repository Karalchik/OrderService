using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces
{
    /// <summary>Persistence contract for the <see cref="Order"/> aggregate.</summary>
    public interface IOrderRepository
    {
        /// <summary>Persists a new order to the database.</summary>
        /// <param name="order">The order entity to persist.</param>
        /// <returns>The persisted order with a generated <see cref="Order.Id"/>.</returns>
        Task<Order> CreateAsync(Order order);

        /// <summary>Replaces an existing order document with updated data.</summary>
        /// <param name="order">The order entity containing updated fields.</param>
        /// <returns>The updated order entity.</returns>
        Task<Order> UpdateAsync(Order order);

        /// <summary>Retrieves a single order by its unique identifier.</summary>
        /// <param name="id">The MongoDB ObjectId as string.</param>
        /// <returns>The matching order, or <c>null</c> if not found.</returns>
        Task<Order?> GetByIdAsync(string id);

        /// <summary>Returns a filtered, paginated list of orders sorted by creation date descending.</summary>
        /// <param name="userName">Filter by user name. Pass <c>null</c> to skip this filter.</param>
        /// <param name="status">Filter by status (case-insensitive). Pass <c>null</c> to skip this filter.</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <param name="offset">Number of results to skip for pagination.</param>
        /// <returns>A collection of matching orders.</returns>
        Task<IEnumerable<Order>> ListAsync(string? userName, string? status, int limit, int offset);
    }
}