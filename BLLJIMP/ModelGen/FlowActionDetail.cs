using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_FlowActionDetail
    /// </summary>
    [Serializable]
    public partial class FlowActionDetail : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 流程执行ID
        /// </summary>
        public int ActionID { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID { get; set; }
        /// <summary>
        /// 环节ID
        /// </summary>
        public int StepID { get; set; }
        /// <summary>
        /// 环节名称
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleDate { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public string HandleUserID { get; set; }
        /// <summary>
        /// 处理内容
        /// </summary>
        public string HandleContent { get; set; }
        /// <summary>
        /// 处理选择时间
        /// </summary>
        public DateTime HandleSelectDate { get; set; }
        /// <summary>
        /// 撤单 
        /// 1退积分给报单人
        /// 管理奖票据
        /// 1发管理奖到余额
        /// </summary>
        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public string Ex3 { get; set; }
        public string WebsiteOwner { get; set; }
    }
}