using Amazon.Domain.Entities;
using Xunit;

namespace Amazon.Domain.Tests
{
    public class CartTests
    {
        [Fact]
        public void AddItem_NewProduct_AddsItemToCart()
        {
            // Arrange
            var cart = new Cart();
            var productId = 1;
            var price = 10.0m;
            var quantity = 2;

            // Act
            cart.AddItem(productId, price, quantity);

            // Assert
            var item = Assert.Single(cart.Items);
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(price, item.UnitPrice);
        }

        [Fact]
        public void AddItem_ExistingProduct_IncreasesQuantity()
        {
            // Arrange
            var cart = new Cart();
            var productId = 1;
            cart.AddItem(productId, 10.0m, 2);

            // Act
            cart.AddItem(productId, 10.0m, 3);

            // Assert
            var item = Assert.Single(cart.Items);
            Assert.Equal(5, item.Quantity);
        }

        [Fact]
        public void RemoveItem_ExistingProduct_RemovesItemFromCart()
        {
            // Arrange
            var cart = new Cart();
            var productId = 1;
            cart.AddItem(productId, 10.0m, 2);

            // Act
            cart.RemoveItem(productId);

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void Clear_RemovesAllItemsFromCart()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(1, 10.0m, 2);
            cart.AddItem(2, 20.0m, 1);

            // Act
            cart.Clear();

            // Assert
            Assert.Empty(cart.Items);
        }
    }
}
