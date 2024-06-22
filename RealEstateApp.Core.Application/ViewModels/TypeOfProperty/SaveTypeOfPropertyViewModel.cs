using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.TypeOfProperty
{
    public class SaveTypeOfPropertyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de tipo de propiedad")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe colocar una descripción de tipo de propiedad")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

    }
}
