using RealEstateApp.Core.Application.Interfaces.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface ITypeOfPropertyService : IGenericService<SaveTypeOfPropertyViewModel, TypeOfPropertyViewModel, TypeOfProperty>
    {
    }
}
