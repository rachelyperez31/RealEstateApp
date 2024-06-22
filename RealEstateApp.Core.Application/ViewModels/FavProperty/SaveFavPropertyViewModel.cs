using RealEstateApp.Core.Application.ViewModels.Property;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.FavProperty
{
    public class SaveFavPropertyViewModel
    {
        [Required(ErrorMessage = "La propiedad debe ser marcada por un usuario.")]
        [DataType(DataType.Text)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Debe elegir una propiedad.")]
        [DataType(DataType.Text)]
        public string PropertyId { get; set; }

        public PropertyViewModel? Property { get; set; }
    }
}
