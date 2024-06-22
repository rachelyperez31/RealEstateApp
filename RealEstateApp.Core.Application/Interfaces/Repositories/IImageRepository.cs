using RealEstateApp.Core.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Task<List<Image>> GetImagesByPropertyId(int propertyId);
    }
}
