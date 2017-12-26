using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.cardcoupon
{

    /// <summary>
    /// 我的卡券-门票
    /// </summary>
   public class MyCardcouponList_EntranceTicket
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<MyCardcoupon_EntranceTicket> list { get; set; }

    }
}
