using RealEstateApp.Core.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyImprovementRepository : IGenericRepository<PropertyImprovement>
    {
        Task<List<PropertyImprovement>> GetPropertyImprovementsByPropertyId(int propertyId);
    }
}
