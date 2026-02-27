using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; } = null!;
    }

    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 100 , MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(maximumLength: 150 , MinimumLength = 1)]
        public string Description { get; set; } = null!;

        [Range(0.01 , double.MaxValue , ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }
    }

    public class UpdateProductDTO
    {
        [Range(0.01 , double.MaxValue , ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }
    }
}
