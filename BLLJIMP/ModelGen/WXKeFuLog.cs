using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信客服号日志
    /// </summary>
    public class WXKeFuLog : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 客服向公众号发送的消息
        /// </summary>
        public string SendMessage { get; set; }
        /// <summary>
        /// 客服OpenID
        /// </summary>
        public string KeFuWeixinOpenId { get; set; }
        /// <summary>
        /// 客服平台登录名
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate { get; set; }


    }
}
