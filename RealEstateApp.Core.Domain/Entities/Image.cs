using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Image : BaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string? ImageUrl { get; set; }
    }
}
