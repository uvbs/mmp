using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_TeamPerformance
    /// </summary>
    [Serializable]
    public partial class TeamPerformance : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        public int YearMonth { get; set; }
        /// <summary>
        /// 用户UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 所属上级
        /// </summary>
        public string DistributionOwner { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 个人业绩
        /// </summary>
        public decimal Performance { get; set; }
        public decimal ChildPerformance { get; set; }
        public decimal TotalPerformance { get; set; }
        /// <summary>
        /// 奖金比例 ChildPerformance 对应比例
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 实得奖励 TotalReward - Reward
        /// </summary>
        public decimal Reward { get; set; }
        /// <summary>
        /// 下级分得奖励 (下级奖励)
        /// </summary>
        public decimal ChildReward { get; set; }
        /// <summary>
        /// 总业绩奖励 (ChildPerformance * Rate)
        /// </summary>
        public decimal TotalReward { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 明细ID
        /// </summary>
        public string DetailIds { get; set; }
        /// <summary>
        /// 管理奖确认ID(流程)
        /// </summary>
        public int FlowActionId { get; set; }
        /// <summary>
        /// 管理奖确认状态(流程)
        /// </summary>
        public int FlowActionStatus { get; set; }
        /// <summary>
        /// 状态
        /// 0待发布 1已发布 2票据审核中 9处理完成
        /// </summary>
        public int Status { get; set; }
    }
}