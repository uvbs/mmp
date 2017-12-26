using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 刮奖活动-奖项设置
    /// </summary>
    public class WXAwards : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 刮奖活动Id 关联表ZCJ_WXLottery AutoID
        /// </summary>
        public int LotteryId { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeName { get; set; }
        /// <summary>
        /// 奖品数量
        /// </summary>
        public int PrizeCount { get; set; }
        /// <summary>
        /// 中奖概率
        /// </summary>
        public string Probability { get; set; }




    }
}
