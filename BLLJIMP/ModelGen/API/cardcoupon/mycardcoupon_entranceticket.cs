using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.cardcoupon
{
    /// <summary>
    /// 我的卡券-门票
    /// </summary>
    public class MyCardcoupon_EntranceTicket:MyCardcouponBase
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname { get; set; }
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
