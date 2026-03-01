using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Domain.Entities
{
    public class Cart
    {
        public int Id { get; private set; }
        public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

        public void AddItem(int productId, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            var existing = Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing == null)
            {
                Items.Add(new CartItem(productId, unitPrice, quantity));
            }
            else
            {
                existing.UpdateQuantity(existing.Quantity + quantity);
            }
        }

        public void UpdateItem(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            var existing = Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing == null)
                throw new InvalidOperationException("Item not found in cart.");

            existing.UpdateQuantity(quantity);
        }

        public void RemoveItem(int productId)
        {
            var existing = Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing == null)
                throw new InvalidOperationException("Item not found in cart.");

            Items.Remove(existing);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
