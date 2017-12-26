using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 申请职位表
    /// </summary>
    public class ApplyPositionInfo : ZCBLLEngine.ModelTable
    {
        public ApplyPositionInfo() { }

        public int AutoId { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        /// 期望薪资
        /// </summary>
        public string ExpectedSalary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ComeTime { get; set; }

        /// <summary>
        /// 离职原因
        /// </summary>
        public string ReasonLeaving { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        public int PositionId { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        ///手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 行业
        /// </summary>
        public string Trade { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Professional { get; set; }

    }
}
