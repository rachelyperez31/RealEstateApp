using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class TypeOfSale : BaseFeature
    {
        public ICollection<Property>? Properties { get; set; }
    }
}
