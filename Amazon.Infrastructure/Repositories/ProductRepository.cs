using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using Amazon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AmazonDbContext context;

        public ProductRepository(AmazonDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public void Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> SearchAsync(
            string? search, 
            int? categoryId, 
            decimal? minPrice, 
            decimal? maxPrice, 
            bool? inStock, 
            string? sortBy, 
            bool desc, 
            int pageNumber, 
            int pageSize)
        {
            var query = context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(s));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            if (inStock.HasValue && inStock.Value)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

            query = (sortBy?.ToLower()) switch
            {
                "name" => desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                _ => query
            };

            var total = await query.CountAsync();
            var skip = (pageNumber - 1) * pageSize;
            var items = await query.Skip(skip).Take(pageSize).ToListAsync();
            return (items, total);
        }
    }
}
