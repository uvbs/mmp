using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 卡券类型
    /// </summary>
    public enum EnumCardCouponType
    {
        /// <summary>
        /// 默认类型
        /// </summary>
        Default,
        /// <summary>
        /// 卡券类型-门票
        /// </summary>
        EntranceTicket,
        /// <summary>
        /// 商城卡券-折扣券 (凭折扣券对指定商品（全场）打折)
        /// </summary>
        MallCardCoupon_Discount,
        /// <summary>
        /// 商城卡券-抵扣券 (支付时可以抵扣现金)
        /// </summary>
        MallCardCoupon_Deductible,
        /// <summary>
        /// 商城卡券-免邮券(满一定金额包邮)
        /// </summary>
        MallCardCoupon_FreeFreight,
        /// <summary>
        /// 商城卡券-满扣券(消费满一定金额减去一定金额。)
        /// </summary>
        MallCardCoupon_Buckle

    }
}
