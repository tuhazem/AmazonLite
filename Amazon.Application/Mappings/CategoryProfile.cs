using Amazon.Application.DTOs;
using Amazon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace Amazon.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {

            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
        }
    }
}
