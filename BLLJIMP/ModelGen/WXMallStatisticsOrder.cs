using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///订单统计
    /// </summary>
    public class WXMallStatisticsOrder : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        ///任务Id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 统计开始日期
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// 统计结束日期
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 总订单数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 订单总基价
        /// </summary>
        public decimal BaseTotalAmount { get; set; }
        /// <summary>
        /// 订单总运费
        /// </summary>
        public decimal TotalTransportFee { get; set; }
        /// <summary>
        /// 总利润=订单总金额-总基价-总运费
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// 优惠券支付
        /// </summary>
        public decimal TotalCouponExchangAmount { get; set; }
        /// <summary>
        /// 积分支付
        /// </summary>
        public decimal TotalScoreExchangAmount { get; set; }
        /// <summary>
        /// 余额支付
        /// </summary>
        public decimal TotalAccountAmountExchangAmount { get; set; }
        /// <summary>
        /// 储值卡支付
        /// </summary>
        public decimal TotalStorecardExchangAmount { get; set; }
        /// <summary>
        /// 商品总件数
        /// </summary>
        public int TotalProductCount { get; set; }
        /// <summary>
        /// 商品应付总金额
        /// </summary>
        public decimal TotalProductFee { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal TotalRefundAmount { get; set; }

        /// <summary>
        /// 应该分佣订单订单数
        /// </summary>
        public int ShouldCommissionOrderCount { get; set; }
        /// <summary>
        /// 实际分佣订单订单数
        /// </summary>
        public int RealCommissionOrderCount { get; set; }
        /// <summary>
        /// 最后一个订单确认收货时间
        /// </summary>
        public string LastReceivingTime { get; set; }
        /// <summary>
        /// 订单交易成功且在退款中的订单数
        /// </summary>
        public int RefundOrderCount { get; set; }
        /// <summary>
        /// 待处理订单数
        /// </summary>
        public int WaitProcessOrderCount { get; set; }
    }
}
