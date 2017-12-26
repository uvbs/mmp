using System;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Statuses
{
    /// <summary>
    /// 单条微博内容
    /// </summary>
    [Serializable]
    public class Show
    {
        /// <summary>
        /// 微博创建时间
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; internal set; }
        /// <summary>
        /// 微博ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ID { get; internal set; }
        /// <summary>
        /// 微博信息内容
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; internal set; }
        /// <summary>
        /// 微博来源
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; internal set; }
        /// <summary>
        /// 是否已收藏，true：是，false：否
        /// </summary>
        [JsonProperty(PropertyName = "favorited")]
        public bool Favorited { get; internal set; }
        /// <summary>
        /// 是否被截断，true：是，false：否
        /// </summary>
        [JsonProperty(PropertyName = "truncated")]
        public bool Truncated { get; internal set; }
        [JsonProperty(PropertyName = "in_reply_to_status_id")]
        public string InReplyToStuatusID { get; internal set; }
        [JsonProperty(PropertyName = "in_reply_to_user_id")]
        public string InReplyToUserID { get; internal set; }
        [JsonProperty(PropertyName = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; internal set; }
        [JsonProperty(PropertyName = "thumbnail_pic")]
        public string ThumbnailPictureUrl { get; internal set; }
        [JsonProperty(PropertyName = "bmiddle_pic")]
        public string MiddleSizePictureUrl { get; internal set; }
        [JsonProperty(PropertyName = "original_pic")]
        public string OriginalPictureUrl { get; internal set; }
        [JsonProperty(PropertyName = "mid")]
        public string MID { get; internal set; }
        [JsonProperty(PropertyName = "reposts_count")]
        public int RepostsCount { get; internal set; }
        [JsonProperty(PropertyName = "comments_count")]
        public int CommentsCount { get; internal set; }
        [JsonProperty("annotations")]
        public object Annotations { get; internal set; }
        [JsonProperty(PropertyName = "geo")]
        public GeoEntity GEO { get; internal set; }
        [JsonProperty(PropertyName = "user")]
        public Users.Show User { get; internal set; }
        [JsonProperty(PropertyName = "retweeted_status")]
        public Show RetweetedStatus { get; internal set; }
    }
}
