using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Interceptor;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.IoC
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AuditableInterceptor>();

            #region Context
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options =>
                                   options.UseInMemoryDatabase(databaseName: "RealEstateDb"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>((sp, options) =>
                {
                    var interceptor = sp.GetService<AuditableInterceptor>();
                    options.UseSqlServer(configuration.GetConnectionString("Default"),
                                        b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName))
                                        .AddInterceptors(interceptor);
                });
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IFavPropertyRepository,  FavPropertyRepository>();
            services.AddTransient<IImprovementRepository, ImprovementRepository>();
            services.AddTransient<ITypeOfPropertyRepository, TypeOfPropertyRepository>();
            services.AddTransient<ITypeOfSaleRepository, TypeOfSaleRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IPropertyImprovementRepository, PropertyImprovementRepository>();
            #endregion
        }
    }
}
