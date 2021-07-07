using System;

namespace WebService.Core.Mvc.Models
{
    public abstract class Persistence
    {
        /// <summary>
        /// 流水號。會在寫入時自動編號。
        /// </summary>
        public uint Id { get; internal set; }

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
