using RealEstateApp.Core.Application.Interfaces.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.Image;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IImageService : IGenericService<SaveImageViewModel, ImageViewModel, Image>
    {
        Task<List<ImageViewModel>> GetImagesForPropertyId(int propertyId);
    }
}
