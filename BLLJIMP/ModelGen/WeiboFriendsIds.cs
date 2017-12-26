using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户关注的用户UID列表
    /// </summary>
    public class WeiboFriendsIds 
    {
        /// <summary>
        /// ID列表
        /// </summary>
        [JsonProperty("ids")]
        public string[] IDS
        {
            get;
            set;
        }

        /// <summary>
        /// 下一页游标
        /// </summary>
        [JsonProperty("next_cursor")]
        public int NextCursor
        {
            get;
            set;
        }

        /// <summary>
        /// 上一页游标
        /// </summary>
        [JsonProperty("previous_cursor")]
        public int PreviousCursor
        {
            get;
            set;
        }

        /// <summary>
        /// 总数量
        /// </summary>
        [JsonProperty("total_number")]
        public int TotalNumber
        {
            get;
            set;
        }

    }
}
