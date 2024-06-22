using RealEstateApp.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class PropertyAgentViewModel
    {
        public PropertyViewModel Propiedad { get; set; }
        public UserViewModel Agent { get; set; }
    }
}
