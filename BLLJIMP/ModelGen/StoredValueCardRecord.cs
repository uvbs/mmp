using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 储值卡发放表
    /// </summary>
    [Serializable]
    public partial class StoredValueCardRecord : ModelTable
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
        /// 卡编号
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// 账面金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidTo { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 转赠用户
        /// </summary>
        public string ToUserId { get; set; }
        /// <summary>
        /// 转赠时间
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// 状态
        /// 0已发送
        /// 1已转赠
        /// 9已使用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseDate { get; set; }
        /// <summary>
        /// 发放人
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}