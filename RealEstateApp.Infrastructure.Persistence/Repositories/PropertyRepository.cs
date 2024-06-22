using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyRepository(ApplicationContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Property>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _dbContext.Set<Property>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            var propertiesResult = await query.Where(q => !q.IsDeleted && 
                                                          !q.TypeOfProperty.IsDeleted &&
                                                          !q.TypeOfSale.IsDeleted &&
                                                          q.Improvements.Any(i => !i.IsDeleted)).ToListAsync();

            return propertiesResult;
        }
    }
}
