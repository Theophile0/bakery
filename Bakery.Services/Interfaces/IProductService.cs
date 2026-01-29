using Bakery.Domains.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsForPublicAsync(); // only active
        Task<Product?> GetProductAsync(int id);
        Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync();

        Task<int> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DisableProductAsync(int id); // soft-hide
        }
}
