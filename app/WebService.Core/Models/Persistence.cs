using System;

namespace WebService.Core.Models
{
    public abstract class Persistence
    {
        /// <summary>
        /// 流水號。會在寫入時自動編號。
        /// </summary>
        public uint Sn { get; internal set; }

        /// <summary>
        /// 隨機的 GUID。會在寫入時自動產生。
        /// </summary>
        public Guid Uuid { get; internal set; }

        /// <summary>
        /// 建立時間。會在寫入時自動產生。
        /// </summary>
        public DateTime CreateTime { get; internal set; }

        /// <summary>
        /// 修改時間。會在寫入時自動產生。
        /// </summary>
        public DateTime UpdateTime { get; internal set; }
    }
}
