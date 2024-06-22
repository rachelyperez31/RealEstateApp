using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewModels.Image;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class SavePropertyViewModel
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        [Required(ErrorMessage = "Debe colocar el Precio de la Propiedad")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor mayor que 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Debe colocar el numero de Habitaciones de la Propiedad")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de habitaciones debe ser mayor que 0")]
        public int NumberOfRooms { get; set; }
        [Required(ErrorMessage = "Debe colocar el numero de Baños de la Propiedad")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de baños debe ser mayor que 0")]
        public int NumberOfBathrooms { get; set; }
        [Required(ErrorMessage = "Debe colocar el numero de Baños de la Propiedad")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La superficie debe ser un valor mayor que 0")]
        public double Size { get; set; }
        [Required(ErrorMessage = "Debe colocar la Descripción del Propiedad")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Elije un tipo de Propiedad")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de propiedad válido")]
        public int TypeOfPropertyId { get; set; }

        [Required(ErrorMessage = "Elije un tipo de venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de venta válido")]
        public int TypeOfSaleId { get; set; }

        [Required(ErrorMessage = "Elije las Mejoras")]
        public List<int>? ImprovementsId { get; set; }

        public string? ImgUrl { get; set; }

        //public List<IFormFile>? Imagenes { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File2 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File3 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File4 { get; set; }

        public List<ImprovementViewModel>? Improvements { get; set; }
        public List<TypeOfPropertyViewModel>? TypeOfPropertys { get; set; }
        public List<TypeOfSaleViewModel>? TypeOfSales { get; set; }
        public List<ImageViewModel>? ImgUrls { get; set; }
        public ICollection<ImageViewModel>? PreviousImages { get; set; }

        public string? AgentId { get; set; }
    }
}
