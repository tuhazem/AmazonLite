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
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDTO
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
