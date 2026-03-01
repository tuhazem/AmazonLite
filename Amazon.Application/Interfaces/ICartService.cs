using Amazon.Application.DTOs;
using System.Threading.Tasks;

namespace Amazon.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO?> GetCartAsync(int cartId);
        Task<int> CreateCartAsync();
        Task AddItemAsync(int cartId, int productId, int quantity);
        Task UpdateItemAsync(int cartId, int productId, int quantity);
        Task RemoveItemAsync(int cartId, int productId);
        Task ClearAsync(int cartId);
    }
}
