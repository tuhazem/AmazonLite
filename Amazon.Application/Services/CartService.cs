using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<CartDTO?> GetCartAsync(int cartId)
        {
            var cart = await cartRepository.GetByIdAsync(cartId);
            return cart == null ? null : mapper.Map<CartDTO>(cart);
        }

        public async Task<int> CreateCartAsync()
        {
            var cart = new Cart();
            await cartRepository.AddAsync(cart);
            return cart.Id;
        }

        public async Task AddItemAsync(int cartId, int productId, int quantity)
        {
            var cart = await cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new System.Exception("Cart not found");

            var product = await productRepository.GetByIdAsync(productId);
            if (product == null) throw new System.Exception("Product not found");

            cart.AddItem(product.Id, product.Price, quantity);
            cartRepository.Update(cart);
        }

        public async Task UpdateItemAsync(int cartId, int productId, int quantity)
        {
            var cart = await cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new System.Exception("Cart not found");

            cart.UpdateItem(productId, quantity);
            cartRepository.Update(cart);
        }

        public async Task RemoveItemAsync(int cartId, int productId)
        {
            var cart = await cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new System.Exception("Cart not found");

            cart.RemoveItem(productId);
            cartRepository.Update(cart);
        }

        public async Task ClearAsync(int cartId)
        {
            var cart = await cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new System.Exception("Cart not found");

            cart.Clear();
            cartRepository.Update(cart);
        }
    }
}
