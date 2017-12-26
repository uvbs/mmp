using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisHelper.Enums
{

    /// <summary>
    /// RedisKey枚举
    /// </summary>
    public enum RedisKeyEnum
    {
        /// <summary>
        /// 站点信息
        /// </summary>
        WebsiteInfo,
        /// <summary>
        /// 站点域名信息
        /// </summary>
        WebsiteDomainInfo,
        /// <summary>
        /// 权限过滤排除 微信授权
        /// </summary>
        WXModuleFilterInfo,
        /// <summary>
        /// 分享监测表
        /// </summary>
        ShareMonitorInfo,
        /// <summary>
        /// 任务
        /// </summary>
        TimingTask,
        /// <summary>
        /// Socket在线
        /// </summary>
        SocketOnline
    }
}
