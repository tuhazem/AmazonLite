using Amazon.Domain.Entities;
using Xunit;

namespace Amazon.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void UpdatePrice_ValidPrice_UpdatesPrice()
        {
            // Arrange
            var product = new Product("Test Product", "Description", 100.0m, 10, 1);
            var newPrice = 150.0m;

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void UpdatePrice_NegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product("Test Product", "Description", 100.0m, 10, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => product.UpdatePrice(-10.0m));
        }

        [Fact]
        public void ReduceStock_ValidQuantity_ReducesStock()
        {
            // Arrange
            var product = new Product("Test Product", "Description", 100.0m, 10, 1);
            var reduceBy = 3;

            // Act
            product.ReduceStock(reduceBy);

            // Assert
            Assert.Equal(7, product.StockQuantity);
        }

        [Fact]
        public void ReduceStock_InsufficientStock_ThrowsInvalidOperationException()
        {
            // Arrange
            var product = new Product("Test Product", "Description", 100.0m, 5, 1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => product.ReduceStock(10));
        }
    }
}
