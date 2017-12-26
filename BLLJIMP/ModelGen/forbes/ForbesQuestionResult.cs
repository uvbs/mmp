using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model.Forbes
{
    /// <summary>
    /// ZCJ_ForbesQuestionResult
    /// </summary>
    [Serializable]
    public partial class ForbesQuestionResult : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public long AutoID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 第几次答题
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// 答题时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 核销码
        /// </summary>
        public string CancelCode { get; set; }
        /// <summary>
        /// 兑换编码
        /// </summary>
        public string GiftCode { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 状态 0未答 1已答
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 活动名
        /// </summary>
        public string Activity { get; set; }
        /// <summary>
        /// 编辑时间（一般作为核销时间）
        /// </summary>
        public DateTime ModifyDate { get; set; }
        
        
    }
}