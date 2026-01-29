using Bakery.Data;
using Bakery.Domains.Data.Entities;
using Bakery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repositories.Implementation
{
    public sealed class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductCategoryRepository(ApplicationDbContext db) => _db = db;

        public Task<ProductCategory?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.ProductCategories.FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task<ProductCategory?> GetByNameAsync(string name, CancellationToken ct = default)
            => _db.ProductCategories.FirstOrDefaultAsync(c => c.Name == name, ct);

        public async Task<IReadOnlyList<ProductCategory>> GetAllAsync(bool onlyActive = true, CancellationToken ct = default)
        {
            var q = _db.ProductCategories.AsNoTracking().AsQueryable();
            if (onlyActive) q = q.Where(c => c.IsActive);

            return await q
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .ToListAsync(ct);
        }

        public async Task AddAsync(ProductCategory category, CancellationToken ct = default)
        {
            _db.ProductCategories.Add(category);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(ProductCategory category, CancellationToken ct = default)
        {
            _db.ProductCategories.Update(category);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(ProductCategory category, CancellationToken ct = default)
        {
            _db.ProductCategories.Remove(category);
            await _db.SaveChangesAsync(ct);
        }

        public Task<ProductCategory?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync(bool onlyActive = true)
        {
            try
            {
                return await _db.ProductCategories.ToListAsync();


            }
            catch (Exception e)
            {
                throw new Exception(e + "Error in ProductCategoryRepository getAllAsync()");
            }
        }

        public Task AddAsync(ProductCategory category)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ProductCategory category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ProductCategory category)
        {
            throw new NotImplementedException();
        }
    }
}
