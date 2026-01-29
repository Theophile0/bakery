using Bakery.Domains.Data.Entities;

namespace Bakery.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(bool onlyActive = true);

        Task AddAsync(Product category);
        Task UpdateAsync(Product category);
        Task DeleteAsync(Product category);
    }
}