using Microsoft.Extensions.Time.Testing;
using Moq;
using OrderService.Application.DTOs;
using OrderService.Application.Mapping;
using OrderService.Application.Services;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using Xunit;

namespace OrderService.Tests
{
    /// <summary>Unit tests for <see cref="OrderCommandService"/>.</summary>
    public class OrderCommandServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly FakeTimeProvider _fakeTime;
        private readonly OrderCommandService _service;

        public OrderCommandServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _fakeTime = new FakeTimeProvider(new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero));
            _service = new OrderCommandService(_repositoryMock.Object, _fakeTime, new OrderMapper());
        }

        /// <summary>Verifies that canceling a delivered order throws <see cref="InvalidOperationException"/>.</summary>
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

        /// <summary>Verifies that canceling a non-existent order throws <see cref="KeyNotFoundException"/>.</summary>
        [Fact]
        public async Task CancelOrder_ShouldThrowKeyNotFound_WhenOrderDoesNotExist()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync("nonexistent"))
                           .ReturnsAsync((Order?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.CancelOrderAsync("nonexistent"));
        }

        /// <summary>Verifies that creating an order calls the repository exactly once.</summary>
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