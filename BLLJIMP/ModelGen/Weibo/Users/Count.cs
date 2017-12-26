using System;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Users
{
    /// <summary>
    /// 用户的粉丝数、关注数、微博数
    /// </summary>
    [Serializable]
    public class Count
    {
        /// <summary>
        /// 微博ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; internal set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        [JsonProperty("followers_count")]
        public string FollowerCount { get; internal set; }
        /// <summary>
        /// 关注数
        /// </summary>
        [JsonProperty("friends_count")]
        public string FriendCount { get; internal set; }
        /// <summary>
        /// 微博数
        /// </summary>
        [JsonProperty("statuses_count")]
        public string StatusCount { get; internal set; }
    }
}
