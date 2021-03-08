using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Core.Mvc.Attributes
{
    public class NamespaceRoute : Attribute, IControllerModelConvention
    {
        private readonly string _baseNamespace;

        public NamespaceRoute(string baseNamespace)
        {
            _baseNamespace = baseNamespace;
        }

        public void Apply(ControllerModel controller)
        {
            // 1. 倘若此控制器有設置屬性路由 ( Route[...] )則忽略處裡
            var hasRouteAttributes = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);
            if (hasRouteAttributes)
            {
                return;
            }

            // 2. 取得控制器的 Namespace 構成新的屬性路由
            var namespc = controller.ControllerType.Namespace;
            if (namespc == null)
                return;
            var template = new StringBuilder();
            template.Append(namespc, _baseNamespace.Length + 1,
                            namespc.Length - _baseNamespace.Length - 1);
            template.Replace('.', '/');
            template.Append("/[controller]/{id?}");

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = template.ToString()
                };
            }
        }
    }
}
