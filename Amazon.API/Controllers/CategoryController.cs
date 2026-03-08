using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
        {
            this.service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var categories = await service.GetAllAsync();
            return Ok(categories);

        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await service.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO categoryCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await service.AddAsync(categoryCreateDto);
            return CreatedAtAction(nameof(GetById), new { id }, null);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO categoryUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await service.UpdateAsync(id, categoryUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.Delete(id);
            return NoContent();

        }
    }
}
