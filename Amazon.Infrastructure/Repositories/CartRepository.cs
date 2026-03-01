using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using Amazon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amazon.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AmazonDbContext context;

        public CartRepository(AmazonDbContext context)
        {
            this.context = context;
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cart cart)
        {
            await context.Carts.AddAsync(cart);
            await context.SaveChangesAsync();
        }

        public void Update(Cart cart)
        {
            context.Carts.Update(cart);
            context.SaveChanges();
        }
    }
}
