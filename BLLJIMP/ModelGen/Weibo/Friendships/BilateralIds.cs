using System;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Friendships
{
    /// <summary>
    /// 用户双向关注的用户ID列表，即互粉UID列表
    /// </summary>
    public class BilateralIds
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ids")]
        public long[] IDs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }
    }
}
