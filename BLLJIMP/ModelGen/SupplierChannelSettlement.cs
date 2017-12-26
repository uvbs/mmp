using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 商户渠道结算
    /// </summary>
    public class SupplierChannelSettlement : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 商户渠道账号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 商户渠道名称
        /// </summary>
        public string ChannelName { get; set; }
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
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 结算单号
        /// </summary>
        public string SettlementId { get; set; }
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
    }
}
