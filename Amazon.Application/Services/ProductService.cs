using Amazon.Application.DTOs;
using Amazon.Application.Interfaces;
using Amazon.Domain.Entities;
using Amazon.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productrepo;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productrepo , IMapper mapper)
        {
            this.productrepo = productrepo;
            this.mapper = mapper;
        }

        public async Task<int> CreateProductAsync(CreateProductDTO product)
        {
            var newproduct = mapper.Map<Product>(product);
            await productrepo.AddAsync(newproduct);
            return newproduct.Id;
        }
        public async Task<PagedResult<ProductDTO>> SearchProductsAsync(ProductListQuery query)
        {
            var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;
            var desc = query.SortDir?.ToLower() == "desc";
            
            var (items, total) = await productrepo.SearchAsync(
                query.Search, 
                query.CategoryId, 
                query.MinPrice, 
                query.MaxPrice, 
                query.InStock, 
                query.SortBy, 
                desc, 
                pageNumber, 
                pageSize);

            return new PagedResult<ProductDTO>
            {
                Items = mapper.Map<IEnumerable<ProductDTO>>(items),
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await productrepo.GetByIdAsync(id);
            if(product == null) throw new KeyNotFoundException("Product not found");

            productrepo.Delete(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await productrepo.GetAllAsync();
            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await productrepo.GetByIdAsync(id);
            return product == null ? null : mapper.Map<ProductDTO>(product);
        }

        public async Task UpdateProductPriceAsync(int id, decimal newprice)
        {
            var product = await productrepo.GetByIdAsync(id);
            if(product == null) throw new KeyNotFoundException("Product not found");

            product.UpdatePrice(newprice);
            productrepo.Update(product);
        }
    }
}
