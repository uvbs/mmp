using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Users
{
    /// <summary>
    /// 用户位置信息
    /// </summary>
    public class PlaceShow:Show
    {
        /// <summary>
        /// 最后所在时间
        /// </summary>
        [JsonProperty(PropertyName = "last_at")]
        public DateTime? LastAt { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        [JsonProperty(PropertyName = "distance")]
        public int? Distance { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [JsonProperty(PropertyName = "lon")]
        public string Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [JsonProperty(PropertyName = "lat")]
        public string Lat { get; set; }
    }
}
