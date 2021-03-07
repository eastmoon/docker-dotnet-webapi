using Microsoft.Extensions.DependencyInjection;
using WebService.Core.Configurations;
using WebService.Core.Repositories;
using WebService.Core.Services;


namespace WebService.Core
{
    public class Startup
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // Dependency injection with Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Dependency injection with Services
            // services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();

            return services;
        }
    }
}
