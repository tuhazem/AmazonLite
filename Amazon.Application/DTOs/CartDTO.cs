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
        [Required(ErrorMessage = "ProductId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ProductId.")]
        public int ProductId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDTO
    {
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }
}
