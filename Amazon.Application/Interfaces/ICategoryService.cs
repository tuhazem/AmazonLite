using Amazon.Application.DTOs;
using Amazon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(CreateCategoryDTO category);

        Task UpdateAsync(int id, UpdateCategoryDTO category);
        Task Delete(int id);
    }
}
