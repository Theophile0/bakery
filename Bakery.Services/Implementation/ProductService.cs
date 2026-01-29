using Bakery.Domains.Data.Entities;
using Bakery.Repositories.Interfaces;
using Bakery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Services.Implementation
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _products;
        private readonly IProductCategoryRepository _categories;

        public ProductService(IProductRepository products, IProductCategoryRepository categories)
        {
            _products = products;
            _categories = categories;
        }

        public Task<IEnumerable<ProductCategory>> GetCategoriesAsync()
            => _categories.GetAllAsync(onlyActive: true);

        public Task<IEnumerable<Product>> GetProductsForPublicAsync()
            => _products.GetAllAsync(onlyActive: true);

        public Task<Product?> GetProductAsync(int id)
            => _products.GetByIdAsync(id);

        public async Task<int> CreateProductAsync(Product product)
        {
            // Minimal sanity rules
            if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("Product name is required.");
            if (product.Price < 0) throw new ArgumentException("Price must be >= 0.");

            await _products.AddAsync(product);
            return product.Id;
        }

        public async Task<IReadOnlyList<ProductCategory>> GetAssortimentForPublicAsync(
            bool hideEmptyCategories = true,
            CancellationToken ct = default)
        {
            var categories = await _categories.GetAllAsync(onlyActive: true);
            var products = await _products.GetAllAsync(onlyActive: true);

            // Index products by CategoryId for efficient grouping
            var productsByCategoryId = products
                .Where(p => p.IsActive) // defensive; repo already filters
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => (IReadOnlyList<Product>)g.ToList());

            var result = categories
                .Where(c => c.IsActive) // defensive; repo already filters
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .Select(c =>
                {
                    // IMPORTANT: don't mutate tracked EF entities if these are tracked.
                    // We create a shallow copy so assigning Products doesn't pollute the change tracker.
                    var copy = new ProductCategory
                    {
                        Id = c.Id,
                        Name = c.Name,
                        SortOrder = c.SortOrder,
                        IsActive = c.IsActive,
                        Products = new List<Product>()
                    };

                    if (productsByCategoryId.TryGetValue(c.Id, out var ps))
                        copy.Products = ps.ToList();

                    return copy;
                })
                .Where(c => !hideEmptyCategories || c.Products.Count > 0)
                .ToList();

            return result;
        }

        public Task UpdateProductAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            return _products.UpdateAsync(product);
        }

        public async Task DisableProductAsync(int id)
        {
            var existing = await _products.GetByIdAsync(id) ?? throw new InvalidOperationException("Product not found.");
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            await _products.UpdateAsync(existing);
        }

        public Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync()
        {
            return _categories.GetAllAsync();
        }
    }
}
