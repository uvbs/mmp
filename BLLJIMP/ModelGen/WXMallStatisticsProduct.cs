using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///商品统计
    /// </summary>
    public class WXMallStatisticsProduct : ZCBLLEngine.ModelTable
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
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品总件数
        /// </summary>
        public int ProductTotalCount { get; set; }
        /// <summary>
        /// 总订单数
        /// </summary>
        public int OrderTotalCount { get; set; }
        /// <summary>
        /// 订单总金额 均摊
        /// </summary>
        public decimal OrderTotalAmount { get; set; }
        /// <summary>
        /// 订单总基价
        /// </summary>
        public decimal OrderBaseTotalAmount { get; set; }
        /// <summary>
        /// 总利润 均摊
        /// </summary>
        public decimal Profit { get; set; }
        /// <summary>
        /// 总下单价
        /// </summary>
        public decimal OrderTotalOrderPrice { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal TotalRefundAmount { get; set; }
    }
}
