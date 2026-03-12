using Amazon.Domain.Entities;
using Xunit;

namespace Amazon.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_ValidItem_AddsToItemsAndRecalculatesTotal()
        {
            // Arrange
            var order = new Order();
            var productId = 1;
            var productName = "Product A";
            var price = 50.0m;
            var quantity = 2;

            // Act
            order.AddItem(productId, productName, price, quantity);

            // Assert
            var item = Assert.Single(order.Items);
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(productName, item.ProductName);
            Assert.Equal(price, item.UnitPrice);
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(100.0m, order.Total);
        }

        [Fact]
        public void AddItem_MultipleItems_CalculatesTotalCorrectly()
        {
            // Arrange
            var order = new Order();
            
            // Act
            order.AddItem(1, "Product A", 10.0m, 2); // 20
            order.AddItem(2, "Product B", 30.0m, 1); // 30

            // Assert
            Assert.Equal(2, order.Items.Count);
            Assert.Equal(50.0m, order.Total);
        }

        [Fact]
        public void AddItem_InvalidQuantity_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.AddItem(1, "Product A", 10.0m, 0));
            Assert.Throws<ArgumentException>(() => order.AddItem(1, "Product A", 10.0m, -5));
        }

        [Fact]
        public void UpdateStatus_NewStatus_UpdatesStatus()
        {
            // Arrange
            var order = new Order();
            var newStatus = OrderStatus.Shipped;

            // Act
            order.UpdateStatus(newStatus);

            // Assert
            Assert.Equal(newStatus, order.Status);
        }

        [Fact]
        public void DefaultStatus_IsPending()
        {
            // Arrange & Act
            var order = new Order();

            // Assert
            Assert.Equal(OrderStatus.Pending, order.Status);
        }
    }
}
