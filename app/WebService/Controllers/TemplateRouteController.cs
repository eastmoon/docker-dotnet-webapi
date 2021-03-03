using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebService.Controllers
{
    public class MyApiControllerAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "Demo/Modules/[controller]";
        public int? Order => 2;
        public string Name { get; set; }
    }

    [MyApiControllerAttribute]
    [ApiController]
    public class TemplateRouteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Content("Demo ValuesController for Values GET route : " + id);
        }

        [HttpGet("[action]")]
        public IActionResult Details(int id)
        {
            return Content("Demo ValuesController for Details route : " + id);
        }

        [HttpGet("CustomAction")]
        public IActionResult CustomActionMethod(int id)
        {
            return Content("Demo ValuesController for CustomAction route : " + id);
        }

    }
}
