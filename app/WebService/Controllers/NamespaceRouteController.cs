using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Core.Mvc.Attributes;

namespace WebService.Demo.Modules
{
    [ApiController]
    [NamespaceRoute("WebService", "[controller]")]
    public class NamespaceRouteController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json", "text/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
            return Content("Demo UsersController for User GET route : " + id);
        }
    }
}
