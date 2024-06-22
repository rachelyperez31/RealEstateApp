using RealEstateApp.Core.Application.ViewModels.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.User
{
    public  class AgentInfoViewModel
    {
        public UserViewModel User { get; set; }
        public List<PropertyViewModel> Properties { get; set; }
    }
}
