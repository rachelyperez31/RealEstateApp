using RealEstateApp.Core.Application.ViewModels.Image;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } // Id
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double Size { get; set; }
        public string Description { get; set; }
        public bool IsFav { get; set; }

        public string AgentId { get; set; }
        public int TypeOfPropertyId { get; set; }
        public TypeOfPropertyViewModel? TypeOfProperty { get; set; }
        public int TypeOfSaleId { get; set; }
        public TypeOfSaleViewModel? TypeOfSale { get; set; }
        public ICollection<ImprovementViewModel>? Improvements { get; set; }
        public ICollection<ImageViewModel>? Images { get; set; }
    }
}
