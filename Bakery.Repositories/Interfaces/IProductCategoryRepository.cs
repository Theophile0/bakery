using Bakery.Domains.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repositories.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory?> GetByIdAsync(int id);
        Task<ProductCategory?> GetByNameAsync(string name);
        Task<IEnumerable<ProductCategory>> GetAllAsync(bool onlyActive = true);

        Task AddAsync(ProductCategory category);
        Task UpdateAsync(ProductCategory category);
        Task DeleteAsync(ProductCategory category);
    }
}
