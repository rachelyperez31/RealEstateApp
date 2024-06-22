using RealEstateApp.Core.Application.Interfaces.Services.Generic;
using RealEstateApp.Core.Application.ViewModels.FavProperty;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<SaveImprovementViewModel, ImprovementViewModel, Improvement>
    {
    }
}
