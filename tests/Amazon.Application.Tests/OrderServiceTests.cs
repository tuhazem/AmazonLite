using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Application.Services;
using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace Amazon.Application.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _cartRepoMock = new Mock<ICartRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(
                _orderRepoMock.Object, 
                _cartRepoMock.Object, 
                _productRepoMock.Object, 
                _uowMock.Object, 
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CheckoutAsync_CartExistsAndHasItems_ReturnsOrderDTO()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart();
            var productId = 10;
            var product = new Product("Laptop", "Desc", 1000m, 5, 1);
            cart.AddItem(productId, 1000m, 2);

            _cartRepoMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _productRepoMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            _orderRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<OrderDTO>(It.IsAny<Order>())).Returns(new OrderDTO { Total = 2000m });

            // Act
            var result = await _orderService.CheckoutAsync(cartId);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(2000m);
            _uowMock.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _uowMock.Verify(uow => uow.CommitAsync(), Times.Once);
            product.StockQuantity.Should().Be(3); // Reduced from 5 to 3
            cart.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task CheckoutAsync_CartIsEmpty_ThrowsInvalidOperationException()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart(); // Empty cart

            _cartRepoMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.CheckoutAsync(cartId));
            _uowMock.Verify(uow => uow.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task CheckoutAsync_ProductNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart();
            cart.AddItem(99, 100m, 1);

            _cartRepoMock.Setup(repo => repo.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _productRepoMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.CheckoutAsync(cartId));
            _uowMock.Verify(uow => uow.RollbackAsync(), Times.Once);
        }
    }
}
