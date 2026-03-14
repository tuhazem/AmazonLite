using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository catrepo, IMapper mapper, ILogger<CategoryService> logger)
        {
            _catRepo = catrepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(CreateCategoryDTO category)
        {
            _logger.LogInformation("Adding new category: {CategoryName}", category.Name);
            var cat = _mapper.Map<Category>(category);
            await _catRepo.AddAsync(cat);    
            _logger.LogInformation("Category created successfully with Id: {CategoryId}", cat.Id);
            return cat.Id;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Deleting category: {CategoryId}", id);
            var cat = await _catRepo.GetByIdAsync(id);
            if (cat == null)
            {
                _logger.LogWarning("Delete failed: Category {CategoryId} not found.", id);
                throw new KeyNotFoundException("Category not found");
            }
            if (cat.Products.Any())
            {
                _logger.LogWarning("Delete failed: Category {CategoryId} has {ProductCount} products.", id, cat.Products.Count);
                throw new InvalidOperationException("Category has products and cannot be deleted");
            }

            _catRepo.Delete(cat);
            _logger.LogInformation("Category {CategoryId} deleted successfully.", id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var cats = await _catRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(cats);
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var cat = await _catRepo.GetByIdAsync(id);
            return cat == null ? null : _mapper.Map<CategoryDTO>(cat);
        }

        public async Task UpdateAsync(int id, UpdateCategoryDTO category)
        {
            _logger.LogInformation("Updating category {CategoryId} to {NewName}", id, category.Name);
            var cat = await _catRepo.GetByIdAsync(id);
            if (cat == null)
            {
                _logger.LogWarning("Update failed: Category {CategoryId} not found.", id);
                throw new KeyNotFoundException("Category not found");
            }

            _mapper.Map(category, cat);
            _catRepo.Update(cat);
            _logger.LogInformation("Category {CategoryId} updated successfully.", id);
        }
    }
}
