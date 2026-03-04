using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Amazon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AmazonDbContext context;

        public CategoryRepository(AmazonDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public void Delete(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories.Include(c => c.Products).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Update(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
        }
    }
}
