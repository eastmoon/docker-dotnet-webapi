using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Core.Mvc.Attributes
{
    public class NamespaceRoute : RouteAttribute, IControllerModelConvention
    {
        private readonly string _baseNamespace;
        public NamespaceRoute(string baseNamespace, string template) : base(template)
        {
            _baseNamespace = baseNamespace;
        }

        public void Apply(ControllerModel controller)
        {
            // 1. 取得控制器的 Namespace 構成新的屬性路由
            var namespc = controller.ControllerType.Namespace;
            if (namespc == null)
                return;
            var template = new StringBuilder();
            template.Append(namespc, _baseNamespace.Length + 1,
                            namespc.Length - _baseNamespace.Length - 1);
            template.Replace('.', '/');

            // 2. 自 Controller 取出 Selector 並修改其 AttributeRouteModel 的參數
            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel.Template = template.Append("/" + selector.AttributeRouteModel.Template).ToString();
            }
        }
    }
}
