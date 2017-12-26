using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 交易审核
    /// </summary>
    public class TransfersAudit : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 交易Id
        /// </summary>
        public string TranId { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string TranInfo { get; set; }
        /// <summary>
        /// 类型
        /// MallRefund 商城退款
        /// DistributionWithdraw 分销提现 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 0 待打款
        /// 1 已打款
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 操作人账户
        /// </summary>
        public string OperaUserId { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }



    }
}
