using Amazon.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amazon.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(int id);
        Task AddAsync(Cart cart);
        void Update(Cart cart);
    }
}
