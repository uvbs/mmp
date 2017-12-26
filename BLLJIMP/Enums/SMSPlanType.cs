using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum SMSPlanType
    {
        /// <summary>
        /// 
        /// </summary>
        Now = 0,
        /// <summary>
        /// 
        /// </summary>
        InTime = 1,
        /// <summary>
        /// 
        /// </summary>
        Trigger = 2,
        /// <summary>
        /// 微信模板通知消息：手动给会员推送消息
        /// </summary>
        WXTemplateMsg_Notify = 3,
        /// <summary>
        /// 微信模板通知消息：系统自动推送消息
        /// </summary>
        WXTemplateMsg_Sys_Notify = 4,
        /// <summary>
        /// 微信图文
        /// </summary>
        WxNews = 5,
        /// <summary>
        /// app消息
        /// </summary>
        AppMsg = 6,
        /// <summary>
        /// app和微信消息
        /// </summary>
        AppAndWx = 7

    }
}
