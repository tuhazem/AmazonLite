using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] int? categoryId = null, [FromQuery] string? sortBy = null, [FromQuery] string sortDir = "asc")
        {
            var query = new Amazon.Application.DTOs.ProductListQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                CategoryId = categoryId,
                SortBy = sortBy,
                SortDir = sortDir
            };
            var result = await productService.SearchProductsAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO productdto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var id = await productService.CreateProductAsync(productdto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrice(int id, [FromBody] UpdateProductDTO newprice)
        {
            await productService.UpdateProductPriceAsync(id, newprice.Price);
            return NoContent();
        }
    }
}
