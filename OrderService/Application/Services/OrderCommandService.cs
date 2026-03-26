using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using OrderService.Application.DTOs;

namespace OrderService.Application.Services
{
    public class OrderCommandService
    {
        private readonly IOrderRepository _repository;

        public OrderCommandService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Status = Order.StatusCreated,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _repository.CreateAsync(order);
            return order;
        }

        public async Task<Order?> CancelOrderAsync(string id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return null;

            if (!order.CanBeCanceled())
            {
                throw new InvalidOperationException("Order cannot be canceled in its current state.");
            }

            order.Status = Order.StatusCanceled;
            await _repository.UpdateAsync(order);
            return order;
        }
    }
}