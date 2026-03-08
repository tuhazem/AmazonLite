using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amazon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var id = await cartService.CreateCartAsync();
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cart = await cartService.GetCartAsync(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(int id, [FromBody] AddCartItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await cartService.AddItemAsync(id, dto.ProductId, dto.Quantity);
            return NoContent();
        }

        [HttpPut("{id}/items/{productId}")]
        public async Task<IActionResult> UpdateItem(int id, int productId, [FromBody] UpdateCartItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await cartService.UpdateItemAsync(id, productId, dto.Quantity);
            return NoContent();
        }

        [HttpDelete("{id}/items/{productId}")]
        public async Task<IActionResult> RemoveItem(int id, int productId)
        {
            await cartService.RemoveItemAsync(id, productId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Clear(int id)
        {
            await cartService.ClearAsync(id);
            return NoContent();
        }
    }
}
