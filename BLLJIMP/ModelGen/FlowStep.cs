using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_FlowStep
    /// </summary>
    [Serializable]
    public partial class FlowStep : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID { get; set; }
        /// <summary>
        /// 环节名称
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 环节类型 0正常环节 1选择环节(未实现) 2插入环节(未实现)
        /// </summary>
        public int StepType { get; set; }
        /// <summary>
        /// 开始通知（有无）(未实现)
        /// </summary>
        public int HasStartNotice { get; set; }
        /// <summary>
        /// 结束通知（有无）(未实现)
        /// </summary>
        public int HasEndNotice { get; set; }
        /// <summary>
        /// 通知会员（有无）(未实现)
        /// </summary>
        public int HasNoticeMember { get; set; }
        /// <summary>
        /// 自定义环节ID(未实现)
        /// </summary>
        public int CustomStepID1 { get; set; }
        /// <summary>
        /// 自定义环节2(未实现)
        /// </summary>
        public int CustomStepID2 { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 选择时间（有无）
        /// </summary>
        public int HasSelectDate { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}