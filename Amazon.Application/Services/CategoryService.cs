using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public CategoryService(ICategoryRepository catrepo , IMapper mapper )
        {
            Catrepo = catrepo;
            Mapper = mapper;
        }

        public ICategoryRepository Catrepo { get; }
        public IMapper Mapper { get; }

        public async Task AddAsync(CreateCategoryDTO category)
        {
            var cat = Mapper.Map<Category>(category);
            await Catrepo.AddAsync(cat);    
            
        }

        public async Task Delete(int id)
        {
            var cat = await Catrepo.GetByIdAsync(id);
            if(cat == null) throw new Exception("Category not found");
            if(cat.Products.Any()) throw new Exception("Category has products and cannot be deleted");
            Catrepo.Delete(cat);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var cats = await Catrepo.GetAllAsync();
            return Mapper.Map<IEnumerable<CategoryDTO>>(cats);
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var cat = await Catrepo.GetByIdAsync(id);
            if(cat == null) throw new Exception("Category not found");
            return Mapper.Map<CategoryDTO>(cat);
        }

        public async Task UpdateAsync(int id, UpdateCategoryDTO category)
        {
            var cat = await Catrepo.GetByIdAsync(id);
            if(cat == null) throw new Exception("Category not found");

            Mapper.Map(category, cat);
            Catrepo.Update(cat);
        }
    }
}
