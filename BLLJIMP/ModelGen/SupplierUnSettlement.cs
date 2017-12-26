using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 商家未结算订单
    /// </summary>
    public class SupplierUnSettlement : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public  string OrderId{get;set;}
        /// <summary>
        /// 供应商账号
        /// </summary>
        public string SupplierUserId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 订单下单时间
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 总基价 产品结算额
        /// </summary>
        public decimal BaseAmount { get; set; }
        /// <summary>
        /// 总运费
        /// </summary>
        public decimal TransportFee { get; set; }
        /// <summary>
        /// 总结算金额
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
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal SaleAmount { get; set; }
        /// <summary>
        /// 总服务费
        /// </summary>
        public decimal ServerAmount { get; set; }
        /// <summary>
        /// 是否退款
        /// </summary>
        public int IsRefund { get; set; }


    }
}
