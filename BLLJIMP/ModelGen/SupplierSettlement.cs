using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 商家结算
    /// </summary>
    public class SupplierSettlement : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 供应商账号
        /// </summary>
        public string SupplierUserId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 结算开始日期
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// 结算结束日期
        /// </summary>
        public DateTime ToDate { get; set; }        
        /// <summary>
        /// 结算状态
        /// 待供应商确认
        /// 供应商已确认
        /// 商城已确认
        /// 财务已确认
        /// 财务已打款
        /// 供应商已确认收款
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 结算单号
        /// </summary>
        public string SettlementId { get; set; }
        /// <summary>
        /// 总基价 产品结算额
        /// </summary>
        public decimal TotalBaseAmount { get; set; }
        /// <summary>
        /// 总运费
        /// </summary>
        public decimal TotalTransportFee { get; set; }
        /// <summary>
        /// 退款总金额
        /// </summary>
        public decimal RefundTotalAmount { get; set; }
        /// <summary>
        /// 总结算金额
        /// </summary>
        public decimal SettlementTotalAmount { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal SaleTotalAmount { get; set; }
        /// <summary>
        /// 总服务费
        /// </summary>
        public decimal ServerTotalAmount { get; set; }
        /// <summary>
        /// 是否已经开票
        /// </summary>
        public int IsInvoice { get; set; }

    }
}
