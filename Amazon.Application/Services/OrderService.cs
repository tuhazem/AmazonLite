using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IEmailService _emailService;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger, IEmailService emailService)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<OrderDTO> CheckoutAsync(int cartId, string userEmail)
        {
            _logger.LogInformation("Processing checkout for CartId: {CartId} by User: {UserEmail}", cartId, userEmail);
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var cart = await cartRepository.GetByIdAsync(cartId);
                if (cart == null)
                {
                    _logger.LogWarning("Checkout failed: Cart {CartId} not found.", cartId);
                    throw new KeyNotFoundException("Cart not found");
                }
                if (!cart.Items.Any())
                {
                    _logger.LogWarning("Checkout failed: Cart {CartId} is empty.", cartId);
                    throw new InvalidOperationException("Cart is empty");
                }

                var order = new Order();
                foreach (var item in cart.Items)
                {
                    var product = await productRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        _logger.LogError("Checkout failed: Product {ProductId} not found in catalog.", item.ProductId);
                        throw new KeyNotFoundException("Product not found");
                    }
                    
                    product.ReduceStock(item.Quantity);
                    order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
                }

                await orderRepository.AddAsync(order);
                cart.Clear();
                cartRepository.Update(cart);

                await unitOfWork.CommitAsync();
                _logger.LogInformation("Order {OrderId} successfully created for Cart {CartId}. Total: {Total}", order.Id, cartId, order.Total);

                // Send Confirmation Email
                try
                {
                    var subject = $"Order Confirmation #{order.Id}";
                    var body = $"<h1>Thank you for your order!</h1><p>Your order #{order.Id} has been placed successfully.</p><p>Total Amount: {order.Total:C}</p>";
                    await _emailService.SendEmailAsync(userEmail, subject, body);
                    _logger.LogInformation("Confirmation email sent to {Email}", userEmail);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send confirmation email to {Email}", userEmail);
                    // We don't throw here because the order was already successfully placed
                }

                return mapper.Map<OrderDTO>(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during checkout for Cart {CartId}. Rolling back.", cartId);
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderDTO?> GetOrderAsync(int orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            return order == null ? null : mapper.Map<OrderDTO>(order);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new KeyNotFoundException("Order not found");

            order.UpdateStatus(status);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
