using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 项目提成表预估
    /// </summary>
    public partial class ProjectCommissionEstimate : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 用户名  
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal ProjectAmount { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 提成比例
        /// </summary>
        public double Rate { get; set; }
        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 贡献用户
        /// </summary>
        public string CommissionUserId { get; set; }
        /// <summary>
        /// 项目类型
        /// DistributionOffLine(线下分销)
        /// DistributionOnLine(商城分销)
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 贡献级别
        /// 0 直销
        /// 1 一级分销
        /// </summary>
        public string CommissionLevel { get; set; }
    }
}
