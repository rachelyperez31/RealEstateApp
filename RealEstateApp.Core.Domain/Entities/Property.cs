using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property : BaseEntity
    {
        public string Code { get; set; } 
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double Size { get; set; }
        public string Description { get; set; }

        public string AgentId { get; set; }
        public int TypeOfPropertyId { get; set; }
        public TypeOfProperty TypeOfProperty { get; set; }
        public int TypeOfSaleId { get; set; }
        public TypeOfSale TypeOfSale { get; set; }
        public ICollection<Improvement> Improvements { get; set; }
        public ICollection<Image>? Images { get; set; }
        public List<PropertyImprovement>? PropertyImprovements { get; set; }
    }
}
