using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.cardcoupon
{
    /// <summary>
    /// 我的卡券基类
    /// </summary>
   public class MyCardcouponBase
    {
       /// <summary>
       /// 卡券编号
       /// </summary>
       public int id { get; set; }

       /// <summary>
       /// 优惠券号码
       /// </summary>
       public string card_number { get; set; }
       /// <summary>
       /// 卡券生效日期
       /// </summary>
       public double card_validfrom { get; set; }
       /// <summary>
       /// 卡券生效日期
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
       /// <summary>
       /// 使用状态 0未使用1 已使用 2已经过期
       /// </summary>
       public int status { get; set; }


    }
}
