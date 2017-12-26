using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 储值卡主表
    /// </summary>
    [Serializable]
    public partial class StoredValueCard : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 卡类型
        /// 0 储值卡
        /// </summary>
        public int CardType { get; set; }
        /// <summary>
        /// 验证类型
        /// 0 固定时间过期
        /// 1 固定时间过期
        /// </summary>
        public int ValidType { get; set; }
        /// <summary>
        /// 有效期(固定有效期)
        /// </summary>
        public DateTime? ValidTo { get; set; }
        /// <summary>
        /// 有效天数(发放时计算有效期)
        /// </summary>
        public int? ValidDay { get; set; }
        /// <summary>
        /// 卡名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 最大发放数
        /// </summary>
        public int MaxCount { get; set; }
        /// <summary>
        /// 当前发放数
        /// </summary>
        public int SendCount { get; set; }
        /// <summary>
        /// 背景图
        /// </summary>
        public string BgImage { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 状态
        /// 0可用
        /// 1停用
        /// 2停用发放 但可使用
        /// </summary>
        public int Status { get; set; }
    }
}