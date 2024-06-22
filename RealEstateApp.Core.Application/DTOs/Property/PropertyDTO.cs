using RealEstateApp.Core.Application.DTOs.TypeOfProperty;
using RealEstateApp.Core.Application.DTOs.TypeOfSale;

namespace RealEstateApp.Core.Application.DTOs.Property
{
    public class PropertyDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public TypeOfPropertyDTO TypeOfProperty { get; set; }
        public int TypeOfPropertyId { get; set; }
        public string TypeOfPropertyName { get; set; }
        public TypeOfSaleDTO TypeOfSale { get; set; }
        public int TypeOfSaleId { get; set; }
        public string TypeOfSaleName { get; set; }
        public decimal Price { get; set; }
        public double Size { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathRooms { get; set; }
        public string Description { get; set; }
        public List<string> Improvements { get; set; }
        public string AgentFullName { get; set; }
        public string AgentId { get; set; }
    }
}
