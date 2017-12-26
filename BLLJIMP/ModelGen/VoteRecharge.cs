using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 投票充值设置
    /// </summary>
    public class VoteRecharge : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 投票ID
        /// </summary>
        public int VoteId { get; set; }
        /// <summary>
        /// 票数
        /// </summary>
        public int RechargeCount { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 礼物名称
        /// </summary>
        public string GiftName { get; set; }

        /// <summary>
        /// 礼物介绍
        /// </summary>
        public string GiftDesc { get; set; }


    }
}
