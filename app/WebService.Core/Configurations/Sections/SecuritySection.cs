using System;

namespace WebService.Core.Configurations.Sections
{
    public class SecuritySection
    {
        public Guid NsManagerToken { get; set; }

        public int? PlatformTokenDuration { get; set; }

        public string AesKey { get; set; }

        public string AesIv { get; set; }
    }
}
