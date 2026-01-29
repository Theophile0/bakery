using Bakery.Data;
using Bakery.Domains.Data.Entities;
using Bakery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Repositories.Implementation
{

    public sealed class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }

        async Task<Product?> IProductRepository.GetByIdAsync(int Id)
        {
            try
            {
                return await _applicationDbContext.Products
                    .Where(r => r.Id == Id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetById organisationrequest with Id={Id}, OrganisationRequestDAO", ex);
            }
        }

        async Task<IEnumerable<Product>> IProductRepository.GetAllAsync(bool onlyActive)
        {
            try
            {
                return await _applicationDbContext.Products.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetAllAsync ProductRepository", ex);

            }
        }

        public Task AddAsync(Product category)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Product category)
        {
            throw new NotImplementedException();
        }
    }
}