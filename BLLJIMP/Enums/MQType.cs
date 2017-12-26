using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 消息队列类型枚举
    /// </summary>
    public enum MQType
    {
        /// <summary>
        /// 分销新会员通知，对应有消息队列里面的 DistNewMemberNoticeInfo 类来处理具体消息
        /// </summary>
        DistNewMemberNotice,
        /// <summary>
        /// 问卷统计及计数
        /// </summary>
        QuestionnaireStatistics,
        /// <summary>
        /// 商品浏览统计及计数
        /// </summary>
        ShopDetailOpenStatistics

    }
}
