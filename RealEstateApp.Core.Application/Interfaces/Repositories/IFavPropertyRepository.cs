using RealEstateApp.Core.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IFavPropertyRepository : IGenericRepository<FavProperty>
    {
        Task<List<FavProperty>> GetFavoriteProperties(string clientId);
        Task<FavProperty> GetFavoriteProperty(string clientId, string code);
        Task<bool> IsPropertyFavorite(string clientId, string code);
        Task UpdateAsync(FavProperty favProperty, string clientId, string propertyCode);
    }
}
