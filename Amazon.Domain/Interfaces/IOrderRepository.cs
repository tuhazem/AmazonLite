using Amazon.Domain.Entities;
using System.Threading.Tasks;

namespace Amazon.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
    }
}
