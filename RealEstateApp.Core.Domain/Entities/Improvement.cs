using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Improvement : BaseFeature
    {
        public ICollection<Property>? Properties { get; set; }
        public List<PropertyImprovement>? PropertyImprovements { get; set; }
    }
}
