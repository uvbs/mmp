using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_TeamPerformanceSet
    /// </summary>
    [Serializable]
    public partial class TeamPerformanceSet : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Performance
        /// </summary>
        public decimal Performance { get; set; }
        /// <summary>
        /// RewardRate
        /// </summary>
        public decimal RewardRate { get; set; }
        /// <summary>
        /// WebsiteOwner
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}