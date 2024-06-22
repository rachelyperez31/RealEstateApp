using RealEstateApp.Core.Application.Interfaces.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.FavProperty;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
    {
        Task<List<FavPropertyViewModel>> GetFavProperties(string clientId);
        Task<List<PropertyViewModel>> GetAllByUserId(string id);
        Task<List<PropertyViewModel>> GetAllViewModelWithFilter(FilterPropertyViewModel filters);
        Task ToogleFavStatus(string clientId, string code);
        Task<List<PropertyViewModel>> GetAgentProperties(string agentId);
        Task<int> GetAgentPropertiesCount(string agentId);
        Task<int> GetRegisteredProperties();
        Task AddPropertyImprovements(int propertyId, int improvementId);
        Task RemovePropertyImprovements(int propertyId);
        Task<List<int>> GetPropertyImprovementIds(int propertyId);
        Task<PropertyViewModel> GetPropertyByIdWithIncludes(int id);
    }
}
