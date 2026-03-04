using Amazon.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(CreateProductDTO product);
        Task UpdateProductPriceAsync(int id, decimal newprice);
        Task DeleteProductAsync(int id);
        Task<PagedResult<ProductDTO>> SearchProductsAsync(ProductListQuery query);
    }
}
