using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.cardcoupon
{
   /// <summary>
   /// 卡券类型-门票
   /// </summary>
   public class Cardcoupon_EntranceTicket:CardcouponBase
    {
       /// <summary>
       /// 门票大图
       /// </summary>
       public string card_bigimg { get; set; }
       /// <summary>
       /// 门票详情
       /// </summary>
       public string card_detail { get; set; }

    }
}
