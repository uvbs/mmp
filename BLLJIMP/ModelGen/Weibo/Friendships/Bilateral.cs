using System;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Friendships
{
    /// <summary>
    /// 用户的双向关注列表，即互粉列表
    /// </summary>
    public class Bilateral
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        [JsonProperty("users")]
        public Users.Show[] Users { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }
    }
}
