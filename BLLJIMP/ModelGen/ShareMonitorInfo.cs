using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 分享监测表
    /// </summary>
    public class ShareMonitorInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 监测id
        /// </summary>
        public int MonitorId { get; set; }
        /// <summary>
        /// 监测任务名称
        /// </summary>
        public string MonitorName { get; set; }
        /// <summary>
        /// 监测Url
        /// </summary>
        public string MonitorUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// *分享数
        /// </summary>
        public int ShareCount { get; set; }
        /// <summary>
        /// *阅读数
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        ///创建用户
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 外键id
        /// </summary>
        public string ForeignkeyId { get; set; }
        /// <summary>
        /// 监测类型：article、activity
        /// </summary>
        public string MonitorType { get; set; }
    }
}
