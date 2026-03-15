using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

       public IEnumerable<ProductDTO> Products { get; set; } = null!;

    }

    public class CreateCategoryDTO
    {
        public string Name { get; set; } = null!;
    }

    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = null!;
    }
}
