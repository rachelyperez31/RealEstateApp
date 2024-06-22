using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.ViewModels.FavProperty
{
    public class FavPropertyViewModel
    {
        public string UserId { get; set; }
        public string PropertyId { get; set; }

        public PropertyViewModel? Property { get; set; }
    }
}
