using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; private set; }
        public int CartId { get; private set; }
        public Cart Cart { get; private set; } = null!;

        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public CartItem(int productId, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            UpdateQuantity(quantity);
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            Quantity = quantity;
        }
    }
}
