using Amazon.Application.DTOs;
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
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_productRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductExists_ReturnsProductDTO()
        {
            // Arrange
            var productId = 1;
            var product = new Product("Test", "Desc", 10.0m, 5, 1);
            var productDTO = new ProductDTO { Id = productId, Name = "Test" };

            _productRepoMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDTO>(product))
                .Returns(productDTO);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId);
            _productRepoMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            _productRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.GetProductByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SearchProductsAsync_ReturnsPagedResult()
        {
            // Arrange
            var query = new ProductListQuery { Search = "test", PageNumber = 1, PageSize = 10 };
            var products = new List<Product> { new Product("test", "desc", 10m, 5, 1) };
            var totalCount = 1;
            var productDTOs = new List<ProductDTO> { new ProductDTO { Name = "test" } };

            _productRepoMock.Setup(repo => repo.SearchAsync(
                query.Search, query.CategoryId, query.MinPrice, query.MaxPrice, query.InStock, 
                query.SortBy, false, query.PageNumber, query.PageSize))
                .ReturnsAsync((products, totalCount));

            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDTO>>(products))
                .Returns(productDTOs);

            // Act
            var result = await _productService.SearchProductsAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(totalCount);
        }
    }
}
