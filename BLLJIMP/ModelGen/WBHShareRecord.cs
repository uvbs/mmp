using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 五步会分享记录
    /// </summary>
    public class WBHShareRecord : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 分享ID
        /// </summary>
        public int ShareId { get; set; }
        /// <summary>
        /// 分享类型 文章 活动 职位
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 微信分享类型 0发送给朋友 1分享到朋友圈
        /// </summary>
        public int WeiXinShareType { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get;set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get;set;}
    }
}
