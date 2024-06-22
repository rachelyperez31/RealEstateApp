using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.TypeOfSale
{
    public class SaveTypeOfSaleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de tipo de venta")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe colocar una descripción de tipo de venta")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
