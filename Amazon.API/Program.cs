using Amazon.Application.Mappings;
using Amazon.Application.Services;
using Amazon.Domain.Interfaces;
using Amazon.Infrastructure.Persistence;
using Amazon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Amazon.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(CartProfile));

// Add DbContext
builder.Services.AddDbContext<AmazonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
