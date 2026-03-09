using System.Threading.Tasks;
using Amazon.Application.DTOs;
using Amazon.Domain.Entities;

namespace Amazon.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CheckoutAsync(int cartId);
        Task<OrderDTO?> GetOrderAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}
