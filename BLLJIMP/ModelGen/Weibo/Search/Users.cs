using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Search
{
    /// <summary>
    /// 搜索用户时的联想搜索建议
    /// </summary>
    [Serializable]
    public class User
    {
        [JsonProperty("screen_name")]
        public string ScreenName { get;  set; }
        [JsonProperty("followers_count")]
        public int FollowersCount { get;  set; }
        [JsonProperty("uid")]
        public string UID { get;  set; }
    }
}
