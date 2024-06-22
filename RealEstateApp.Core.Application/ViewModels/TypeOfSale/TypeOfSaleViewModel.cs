using RealEstateApp.Core.Application.ViewModels.Property;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.TypeOfSale
{
    public class TypeOfSaleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PropertyViewModel>? Properties { get; set; }
    }
}
