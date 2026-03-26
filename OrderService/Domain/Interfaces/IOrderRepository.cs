using OrderService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order order);
        Task UpdateAsync(Order order);
        Task<Order?> GetByIdAsync(string id);

        //Чтение с фильтрацией и пагинацией
        Task<IEnumerable<Order>> ListAsync(string? userId, string? status, int limit, int offset);
    }
}