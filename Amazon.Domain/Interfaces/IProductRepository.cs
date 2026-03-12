using Amazon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<(IEnumerable<Product> Items, int TotalCount)> SearchAsync(
            string? search, 
            int? categoryId, 
            decimal? minPrice, 
            decimal? maxPrice, 
            bool? inStock, 
            string? sortBy, 
            bool desc, 
            int pageNumber, 
            int pageSize);
    }
}
