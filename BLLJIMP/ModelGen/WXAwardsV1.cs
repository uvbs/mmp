using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 刮奖活动-奖项设置
    /// </summary>
    public class WXAwardsV1 : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 刮奖活动Id 关联表ZCJ_WXLotteryV1 AutoID
        /// </summary>
        public int LotteryId { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeName { get; set; }
        /// <summary>
        /// 奖品总数量
        /// </summary>
        public int PrizeCount { get; set; }
        /// <summary>
        /// 中奖概率 1-100
        /// </summary>
        public int Probability { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 奖项类型：0为默认自定义，1积分，2优惠券
        /// </summary>
        public int AwardsType { get; set; }
        /// <summary>
        /// 价值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 奖项说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 已经中奖数量
        /// </summary>
        public int WinCount { get; set; }
        
        #region ModelEx
        /// <summary>
        /// 价值的名称，如优惠券的名称
        /// </summary>
        public string ValueName { get; set; }

        #endregion
    }
}
