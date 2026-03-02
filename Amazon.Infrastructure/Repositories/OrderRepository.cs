using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using Amazon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Amazon.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AmazonDbContext context;

        public OrderRepository(AmazonDbContext context)
        {
            this.context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
    }
}
