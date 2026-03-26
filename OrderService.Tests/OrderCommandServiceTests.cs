using Moq;
using OrderService.Application.Services;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using Xunit;

namespace OrderService.Tests
{
    public class OrderCommandServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly OrderCommandService _service;

        public OrderCommandServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _service = new OrderCommandService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CancelOrder_ShouldThrowException_WhenOrderIsDelivered()
        {
            var orderId = "123";
            var deliveredOrder = new Order
            {
                Id = orderId,
                Status = Order.StatusDelivered
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId))
                           .ReturnsAsync(deliveredOrder);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CancelOrderAsync(orderId));
        }

        [Fact]
        public async Task CreateOrder_ShouldCallRepositorySave()
        {
            var request = new OrderService.Application.DTOs.CreateOrderRequest
            {
                UserId = "user1",
                Items = new()
            };

            await _service.CreateOrderAsync(request);

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Once);
        }
    }
}