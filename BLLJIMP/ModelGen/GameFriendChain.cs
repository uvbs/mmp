using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 游戏 朋友链，如HR LOVE 
    /// </summary>
    public class GameFriendChain : ZCBLLEngine.ModelTable
    {
        public int? AutoId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Name { get; set; }

        //用户缩略图地址
        public string ThumbnailUrl { get; set; }

        //照片地址
        public string PhotoUrl { get; set; }

        //星座
        public string StarSign { get; set; }

        //捐款数量
        public double  DonateCount { get; set; }

        //上级用户ID
        public string PreviousUserId { get; set; }

        //下级第一个用户ID
        public string Next1UserId { get; set; }

        //下级第二个用户ID
        public string Next2UserId { get; set; }

        //网站Owner
        public string WebsiteOwner { get; set; }
    }
}
