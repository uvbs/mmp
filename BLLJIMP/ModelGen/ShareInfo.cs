using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 分享表
    /// </summary>
    public class ShareInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int MonitorId { get; set; }
        /// <summary>
        ///分享ID
        /// </summary>
        public string ShareId { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public string PreId { get; set; }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Openid
        /// </summary>
        public string UserWxOpenId { get; set; }
        /// <summary>
        /// 分享时间
        /// </summary>
        public DateTime ShareTime { get; set; }
        /// <summary>
        /// PV
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 子分享数
        /// </summary>
        public int SubShareCount { get; set; }
        /// <summary>
        /// 子分享阅读数
        /// </summary>
        public int SubShareRedCount { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 微信消息类型
        /// </summary>
        public string WxMsgType { get; set; }

    }
}
