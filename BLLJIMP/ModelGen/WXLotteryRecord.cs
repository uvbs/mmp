using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 中奖记录表
    /// </summary>
   public class WXLotteryRecord : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号标识
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 抽奖活动 id
       /// </summary>
       public int LotteryId { get; set; }
       /// <summary>
       /// 用户名
       /// </summary>
       public string UserId { get; set; }

       /// <summary>
       /// 奖项 对应ZCJ_WXAwards
       /// </summary>
       public int WXAwardsId { get; set; }

       /// <summary>
       /// 中奖码
       /// </summary>
       public string Token { get; set; }
       /// <summary>
       /// 中奖日期
       /// </summary>
       public DateTime InsertDate { get; set; }
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
                   return bll.Get<WXAwards>(string.Format("AutoID={0}", WXAwardsId)).PrizeName;

               }
               catch (Exception)
               {

                   return "";
               }

           }

       }

       public string IsGetPrize { get; set; }

    }
}
