using Microsoft.Extensions.Time.Testing;
using Moq;
using OrderService.Application.DTOs;
using OrderService.Application.Services;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using Xunit;

namespace OrderService.Tests
{
    public class OrderCommandServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly FakeTimeProvider _fakeTime;
        private readonly OrderCommandService _service;

        public OrderCommandServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _fakeTime = new FakeTimeProvider(new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero));
            _service = new OrderCommandService(_repositoryMock.Object, _fakeTime);
        }

        [Fact]
        public async Task CancelOrder_ShouldThrowException_WhenOrderIsDelivered()
        {
            var orderId = "123";
            var deliveredOrder = new Order
            {
                Id = orderId,
                UserName = "user1",
                Status = OrderStatus.Delivered
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(orderId))
                           .ReturnsAsync(deliveredOrder);

            _fakeTime.Advance(TimeSpan.FromHours(1));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CancelOrderAsync(orderId));
        }

        [Fact]
        public async Task CreateOrder_ShouldCallRepositorySave()
        {
            var request = new OrderDto
            {
                UserName = "user1",
                Items = new()
            };

            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                           .ReturnsAsync((Order o) => o);

            await _service.CreateOrderAsync(request);

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Once);
        }
    }
}