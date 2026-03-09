using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amazon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("checkout/{cartId}")]
        public async Task<IActionResult> Checkout(int cartId)
        {
            var order = await orderService.CheckoutAsync(cartId);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await orderService.GetOrderAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatus status)
        {
            await orderService.UpdateOrderStatusAsync(id, status);
            return NoContent();
        }
    }
}
