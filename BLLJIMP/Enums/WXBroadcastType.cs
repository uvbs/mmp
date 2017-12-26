using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 广播消息类型
    /// </summary>
    public enum BroadcastType
    {
        /// <summary>
        /// 微信模板消息：通知
        /// </summary>
        WXTemplateMsg_Notify,
        AppMsg,
        AppAndWx
    }
}
