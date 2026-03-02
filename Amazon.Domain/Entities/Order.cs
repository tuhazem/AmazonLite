using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazon.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public decimal Total { get; private set; }
        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        public void AddItem(int productId, string productName, decimal unitPrice, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            Items.Add(new OrderItem(productId, productName, unitPrice, quantity));
            RecalculateTotal();
        }

        void RecalculateTotal()
        {
            Total = Items.Sum(i => i.UnitPrice * i.Quantity);
        }
    }
}
