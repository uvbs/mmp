using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 中奖设置表 
    /// </summary>
    public class WXLotteryWinningData : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 抽奖活动ID 对应ZCJ_WXLottery
        /// </summary>
        public int LotteryId { get; set; }
        /// <summary>
        /// 中奖位置索引
        /// </summary>
        public int WinningIndex { get; set; }
        /// <summary>
        /// 奖品ID 对应ZCJ_WXAwards
        /// </summary>
        public int WXAwardsId { get; set; }


        public string WXAwardName {

            get {

                try
                {
                   BLL bll = new BLL("");
                   return bll.Get<WXAwards>(string.Format("AutoID={0}",WXAwardsId)).PrizeName;

                }
                catch (Exception)
                {

                    return "";
                }
            
            }
        
        }
        



    }
}
