using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class TypeOfProperty : BaseFeature
    {
        public ICollection<Property>? Properties { get; set; }
    }
}
