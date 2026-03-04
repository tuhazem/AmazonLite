using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<OrderDTO> CheckoutAsync(int cartId)
        {
            var cart = await cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new KeyNotFoundException("Cart not found");
            if (!cart.Items.Any()) throw new InvalidOperationException("Cart is empty");

            var order = new Order();
            foreach (var item in cart.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new KeyNotFoundException("Product not found");
                product.ReduceStock(item.Quantity);
                order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
            }

            await orderRepository.AddAsync(order);
            cart.Clear();
            cartRepository.Update(cart);

            return mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO?> GetOrderAsync(int orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            return order == null ? null : mapper.Map<OrderDTO>(order);
        }
    }
}
