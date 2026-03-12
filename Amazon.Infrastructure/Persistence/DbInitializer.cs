using Amazon.Domain.Entities;
using Amazon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AmazonDbContext context)
        {
            // Apply any pending migrations
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            // Seed Categories if none exist
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category("Electronics"),
                    new Category("Books"),
                    new Category("Clothing"),
                    new Category("Home & Kitchen")
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Seed Products if none exist
            if (!context.Products.Any())
            {
                var electronics = await context.Categories.FirstAsync(c => c.Name == "Electronics");
                var books = await context.Categories.FirstAsync(c => c.Name == "Books");

                var products = new List<Product>
                {
                    new Product("Laptop", "Powerful gaming laptop", 1200.00m, 10, electronics.Id),
                    new Product("Smartphone", "Latest model smartphone", 800.00m, 15, electronics.Id),
                    new Product("Headphones", "Noise-cancelling over-ear headphones", 200.00m, 30, electronics.Id),
                    new Product("C# in Depth", "Learn C# like a pro", 45.00m, 50, books.Id),
                    new Product("Clean Code", "A handbook of agile software craftsmanship", 40.00m, 40, books.Id)
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
