using System.Threading.Tasks;
using Amazon.Application.DTOs;

namespace Amazon.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CheckoutAsync(int cartId);
        Task<OrderDTO?> GetOrderAsync(int orderId);
    }
}
