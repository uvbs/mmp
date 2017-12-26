using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 抽奖记录表
    /// </summary>
    public class WXLotteryLog : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号标识
       /// </summary>
       public int AutoID { get; set; }
        /// <summary>
       /// 刮奖活动ID 关联表ZCJ_WXLottery
        /// </summary>
       public int LotteryId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
       public string UserId { get; set; }
       /// <summary>
       /// 刮奖日期
       /// </summary>
       public DateTime InsertDate { get; set; }



    }
}
