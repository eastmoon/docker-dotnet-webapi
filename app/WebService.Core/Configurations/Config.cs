using WebService.Core.Configurations.Sections;

namespace WebService.Core.Configurations
{
    public class Config : IConfig
    {
        public SecuritySection Security { get; set; }
        public ConnectionSection ConnectionStrings { get; set; }
    }
}
