using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 优惠券 旧
    /// </summary>
    public class Coupon : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoId { get; set; }
       /// <summary>
       /// 优惠券号码
       /// </summary>
       public string CouponNumber { get; set; }
       /// <summary>
       /// 创建人用户名
       /// </summary>
       public string CreateUserId { get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>
       public DateTime InsertDate { get; set; }
       /// <summary>
       /// 优惠券类型 0代表商城优惠券
       /// </summary>
       public int CouponType { get; set; }
       /// <summary>
       /// 折扣 1-10
       /// </summary>
       public double Discount { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebSiteOwner { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
       public string ProductId { get; set; }
        /// <summary>
        /// 优惠券生效日期
        /// </summary>
       public string StartDate { get; set; }
        /// <summary>
        /// 优惠券失效日期
        /// </summary>
       public string StopDate { get; set; }



    }
}
