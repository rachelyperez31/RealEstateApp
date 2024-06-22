using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Services.Generic;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services.Generic;
using System.Reflection;
using RealEstateApp.Core.Application.Services;

namespace RealEstateApp.Core.Application.IoC
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<ITypeOfPropertyService, TypeOfPropertyService>();
            services.AddTransient<ITypeOfSaleService, TypeOfSaleService>();
            services.AddTransient<IImageService, ImageService>();
            #endregion
        }
    }
}
