using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 活动抽奖参与者信息
    /// </summary>
    public class LotteryUserInfo:ModelTable
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 关联id   抽奖活动id
        /// </summary>
        public int LotteryId { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        public string WXHeadimgurl { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WXNickname { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime WinnerDate { get; set; }
        /// <summary>
        /// 是否中奖 0未中奖 1已中奖
        /// </summary>
        public int IsWinning { get; set; }

        /// <summary>
        /// 中奖编号
        /// </summary>
        public int Number { get; set; }
    }
}
