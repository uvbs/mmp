using System;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Friendships
{
    /// <summary>
    /// 用户的关注列表
    /// </summary>
    public class Friends
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        [JsonProperty("users")]
        public Users.Show[] UserList { get; set; }
        /// <summary>
        /// 下一页游标
        /// </summary>
        [JsonProperty("next_cursor")]
        public int NextCursor { get; set; }
        /// <summary>
        /// 上一页游标
        /// </summary>
        [JsonProperty("previous_cursor")]
        public int PreviousCursor { get; set; }
        /// <summary>
        /// 关注总数
        /// </summary>
        [JsonProperty("total_number")]
        public int total_number { get; set; }
    }
}
