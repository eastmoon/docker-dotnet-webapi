using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Core.Mvc.Attributes;
using WebService.Core.Mvc.Controllers;
using WebService.Core.Mvc.Models;
using WebService.Core.Services;
using WebService.Core.Repositories;
using WebService.Entities.Models;
using WebService.Entities.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebService.Controllers
{
    public class UsersRequest
    {
        public string Name { get; set; }
    }
    public class UsersResponse : UsersRequest
    {
        public Guid Uuid { get; set; }
    }
    public class UsersModel : Persistence
    {
        public string Name { get; set; }
    }
    public class UsersRepository : CRUDRepository<Users>
    {
        public UsersRepository(
            QueryDBContext queryContext, 
            CommandDBContext commandContext
            )
            : base(queryContext, commandContext)
        {
                
        }
    }
    public class UsersService : CRUDService<UsersModel, Users>
    {
        public UsersService(
            CRUDRepository<Users> repository,
            IUnitOfWork unitOfWork
            )
            : base(repository, unitOfWork)
        {

        }
    }

    [ApiController]
    [NamespaceRoute("WebService", "[controller]")]
    public class UsersController : WSCRUDControllerBase<UsersRequest, UsersResponse, UsersModel>
    {
        public UsersController(
            ILogger<UsersController> logger,
            CRUDService<UsersModel, Users> service
            )
        : base(logger, service)
        {

        }
    }

    public static class UsersServiceStartup
    {
        /// <summary>
        /// 設定 WebService.Controller 的 Users 模組定義的服務。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWSUsersService(this IServiceCollection services)
        {
            // Dependency injection with Repositories
            services.AddScoped<CRUDRepository<Users>, UsersRepository>();

            // Dependency injection with Services
            services.AddScoped<CRUDService<UsersModel, Users>, UsersService>();

            return services;
        }
    }
}
