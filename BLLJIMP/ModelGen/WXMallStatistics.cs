using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///店铺统计
    /// </summary>
    public class WXMallStatistics : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 统计日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 成交笔数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 成交件数
        /// </summary>
        public int OrderProuductTotalCount { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal OrderTotalAmount { get; set; }
        /// <summary>
        /// 当日退货件数
        /// </summary>
        public int RefundProductTotalCount { get; set; }
        /// <summary>
        /// 当日退货金额
        /// </summary>
        public decimal RefundTotalAmount { get; set; }
        /// <summary>
        /// PV
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// UV
        /// </summary>
        public int UV { get; set; }
        /// <summary>
        /// 在线商品数
        /// </summary>
        public int ProductTotalCount { get; set; }
        /// <summary>
        /// 转化率
        /// </summary>
        public string ConvertRate { get; set; }
        /// <summary>
        /// 客单价
        /// </summary>
        public decimal PerCustomerTransaction{get;set;}
        /// <summary>
        /// 商品平均单价
        /// </summary>
        public decimal ProcuctAveragePrice { get; set; }
        /// <summary>
        /// 月累积
        /// </summary>
        public decimal OrderTotalAmountMonth { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 销售总额
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal InvoiceAmount { get; set; }

        /// <summary>
        /// 商户结算总额
        /// </summary>
        public decimal MerchantSettlemenTotalAmount { get; set; }

    }
}
