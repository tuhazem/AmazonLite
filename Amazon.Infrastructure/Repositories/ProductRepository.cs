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

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
        }
    }
}
