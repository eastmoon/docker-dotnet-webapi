using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebService.Core.Configurations;
using WebService.Core.Repositories;
using WebService.Core.Services;
using WebService.Entities.Context;

namespace WebService.Core
{
    public static class Startup
    {
        /// <summary>
        /// 設定 WebService.Core 定義的服務。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWSCoreService(this IServiceCollection services)
        {
            // Dependency injection with Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Dependency injection with Services
            // services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();

            return services;
        }
        /// <summary>
        /// 設定 WebService.Core 定義的組態檔。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddWSCoreConfig(this IServiceCollection services, IConfiguration configuration, out IConfig config)
        {
            config = new Config();

            configuration.Bind(config);

            services.AddSingleton(config);

            return services;
        }
        /// <summary>
        /// 依據 WebService.Core 組態檔來設定 WebService.Entities 所需的資料庫物件。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddWSCoreDbContext(this IServiceCollection services, IConfig config)
        {
            services.AddDbContextPool<CommandDBContext>(options => {
                options.UseMySql(config.ConnectionStrings.Command);
            });
            services.AddDbContextPool<QueryDBContext>(options => {
                options.UseMySql(config.ConnectionStrings.Query);
            });

            return services;
        }
    }
}
