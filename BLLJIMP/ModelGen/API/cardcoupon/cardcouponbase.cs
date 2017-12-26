using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.cardcoupon
{
    /// <summary>
    /// 主卡券基类
    /// </summary>
   public class CardcouponBase
    {
       /// <summary>
       /// 编号
       /// </summary>
       public int card_id { get; set; }
       /// <summary>
       /// 卡券生效日期
       /// </summary>
       public double card_validfrom { get; set; }
       /// <summary>
       /// 卡券失效日期
       /// </summary>
       public double card_validto { get; set; }
       /// <summary>
       /// 卡券名称
       /// </summary>
       public string card_name { get; set; }
       /// <summary>
       /// 卡券LOGO
       /// </summary>
       public string card_logo { get; set; }



    }
}
