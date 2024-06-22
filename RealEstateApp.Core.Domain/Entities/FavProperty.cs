using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class FavProperty : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public string PropertyCode { get; set; }

        public Property? Property { get; set; }
    }
}
