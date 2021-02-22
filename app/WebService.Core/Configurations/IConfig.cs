using WebService.Core.Configurations.Sections;

namespace WebService.Core.Configurations
{
    public interface IConfig
    {
        SecuritySection Security { get; set; }

        ConnectionSection ConnectionStrings { get; set; }
    }
}
