using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 默认中奖设置表 
    /// </summary>
    public class WXLotteryWinningDataV1 : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 抽奖活动ID 对应ZCJ_WXLotteryV1
        /// </summary>
        public int LotteryId { get; set; }
        /// <summary>
        /// 中奖用户名
        /// </summary>
        public string  UserId { get; set; }
        /// <summary>
        /// 奖品ID 对应ZCJ_WXAwardsV1
        /// </summary>
        public int WXAwardsId { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string WXAwardName
        {
            get
            {
                try
                {
                    BLL bll = new BLL("");
                    return bll.Get<WXAwardsV1>(string.Format("AutoID={0}", WXAwardsId)).PrizeName;

                }
                catch (Exception)
                {

                    return "";
                }

            }
        }


    }
}
