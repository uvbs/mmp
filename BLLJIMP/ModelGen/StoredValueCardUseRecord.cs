using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 储值卡使用记录
    /// </summary>
    [Serializable]
    public partial class StoredValueCardUseRecord : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 主卡id
        /// </summary>
        public int CardId { get; set; }
        /// <summary>
        /// 我的卡券Id
        /// </summary>
        public int MyCardId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 原始用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 使用用户Id
        /// </summary>
        public string UseUserId { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseDate { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 使用的金额
        /// </summary>
        public decimal UseAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}