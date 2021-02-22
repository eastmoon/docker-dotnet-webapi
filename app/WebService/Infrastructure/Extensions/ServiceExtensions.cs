using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebService.Core.Configurations;
using WebService.Entities.Context;

namespace WebService.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 注入自定義的組態檔。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="nsConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfig(this IServiceCollection services, IConfiguration configuration, out IConfig config)
        {
            config = new Config();

            configuration.Bind(config);

            services.AddSingleton(config);

            return services;
        }

        /// <summary>
        /// 注入讀寫分離的資料庫物件。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="nsConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfig config)
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
