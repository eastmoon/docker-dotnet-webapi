using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Core.Attributes;
using WebService.Core.Controllers;
using WebService.Core.Models;
using WebService.Core.Services;
using WebService.Core.Repositories;
using WebService.Entities.Models;
using WebService.Entities.Context;

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
        public UsersRepository(QueryDBContext queryContext, CommandDBContext commandContext)
            : base(queryContext, commandContext)
        {
                
        }
    }
    public class UsersService : CRUDService<UsersModel, Users>
    {
        public UsersService(CRUDRepository<Users> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {

        }
    }
    [NamespaceRoute("WebService")]
    public class UsersController : WSCRUDControllerBase<UsersRequest, UsersResponse, UsersModel>
    {
        public UsersController(
            ILogger<UsersController> logger,
            UsersService service
            )
        : base(logger, service)
        {

        }
    }
}
