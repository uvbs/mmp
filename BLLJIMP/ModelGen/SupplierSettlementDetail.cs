using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 商家结算详细
    /// </summary>
    public class SupplierSettlementDetail : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 结算单号
        /// </summary>
        public string SettlementId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 基价
        /// </summary>
        public decimal BaseAmount { get; set; }
       /// <summary>
       /// 运费
       /// </summary>
        public decimal TransportFee { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal SettlementAmount { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime InsertDate { get; set; }

    }
}
