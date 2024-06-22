using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RealEstateApp.Core.Application.ViewModels.Image
{
    public class SaveImageViewModel
    {
        public int? Id { get; set; }
        public int PropertyId { get; set; }
        public string UserId { get; set; }
        public string? ImageUrl { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
