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
    public class ProductService : IProductService
    {
        private readonly IProductRepository productrepo;
        private readonly IMapper mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productrepo, IMapper mapper, ILogger<ProductService> logger)
        {
            this.productrepo = productrepo;
            this.mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateProductAsync(CreateProductDTO product)
        {
            _logger.LogInformation("Creating new product: {ProductName}", product.Name);
            var newproduct = mapper.Map<Product>(product);
            await productrepo.AddAsync(newproduct);
            _logger.LogInformation("Product created successfully with Id: {ProductId}", newproduct.Id);
            return newproduct.Id;
        }

        public async Task<PagedResult<ProductDTO>> SearchProductsAsync(ProductListQuery query)
        {
            _logger.LogInformation("Searching products with query: {@Query}", query);
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

            _logger.LogInformation("Search completed. Found {Count} items.", total);

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
            _logger.LogInformation("Deleting product: {ProductId}", id);
            var product = await productrepo.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Delete failed: Product {ProductId} not found.", id);
                throw new KeyNotFoundException("Product not found");
            }

            productrepo.Delete(product);
            _logger.LogInformation("Product {ProductId} deleted successfully.", id);
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
            _logger.LogInformation("Updating price for product {ProductId} to {NewPrice}", id, newprice);
            var product = await productrepo.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Update price failed: Product {ProductId} not found.", id);
                throw new KeyNotFoundException("Product not found");
            }

            product.UpdatePrice(newprice);
            productrepo.Update(product);
            _logger.LogInformation("Product {ProductId} price updated successfully.", id);
        }
    }
}
