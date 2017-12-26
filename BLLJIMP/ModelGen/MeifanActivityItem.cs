using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 美帆活动选项 (活动,比赛,培训共用) 支付选项
    /// </summary>
    public class MeifanActivityItem : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string FromDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string ToDate { get; set; }
        /// <summary>
        /// 组别
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        ///会员类型
        /// </summary>
        public string IsMember { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
