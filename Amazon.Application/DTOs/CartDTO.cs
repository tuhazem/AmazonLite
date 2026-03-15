using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Application.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public IEnumerable<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class AddCartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDTO
    {
        public int Quantity { get; set; }
    }
}
