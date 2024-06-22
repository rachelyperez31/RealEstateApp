using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.Core.Application.ViewModels.TypeOfProperty
{
    public class TypeOfPropertyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PropertyViewModel>? Properties { get; set; }
    }
}
