using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyImprovementRepository : GenericRepository<PropertyImprovement>, IPropertyImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<PropertyImprovement> AddAsync(PropertyImprovement entity)
        {
            var existingPropertyImprovement = await _dbContext.Set<PropertyImprovement>()
                .FirstOrDefaultAsync(pi => pi.PropertyId == entity.PropertyId 
                                            && pi.ImprovementId == entity.ImprovementId);

            if (existingPropertyImprovement == null)
            {
                return await base.AddAsync(entity);
            }
            else
            {
                existingPropertyImprovement.IsDeleted = false;
                return existingPropertyImprovement;
            }
        }

        public async Task<List<PropertyImprovement>> GetPropertyImprovementsByPropertyId(int propertyId)
        {
            var propertyImprovements = await _dbContext.Set<PropertyImprovement>()
                                                            .Where(pi => pi.PropertyId == propertyId)
                                                            .ToListAsync();

            return propertyImprovements;
        }
    }
}
