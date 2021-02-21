using System;
using Microsoft.Extensions.DependencyInjection;
using WebService.Core.Repositories;
using WebService.Core.Services;


namespace WebService.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Dependency injection with Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Dependency injection with Services
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
        }
    }
}
