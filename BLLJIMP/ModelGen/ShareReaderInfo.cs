using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 分享阅读表
    /// </summary>
    public class ShareReaderInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 分享ID
        /// </summary>
        public string ShareId { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Openid
        /// </summary>
        public string UserWxOpenId { get; set; }
        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime ReadTime { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// IP 所在地
        /// </summary>
        public string IPLocation { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string EventBrowser { get; set; }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string EventBrowserID { get; set; }
        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string EventBrowserVersion { get; set; }
        /// <summary>
        /// 浏览器是否测试版
        /// </summary>
        public string EventBrowserIsBata { get; set; }
        /// <summary>
        /// 系统平台 windows 
        /// </summary>
        public string EventSysPlatform { get; set; }
        /// <summary>
        /// 系统位数 32 64
        /// </summary>
        public string EventSysByte { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string EventUserAgent { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int MonitorId { get; set; }
    }
}
