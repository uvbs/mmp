using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_FlowActionFile
    /// </summary>
    [Serializable]
    public partial class FlowActionFile : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 执行ID
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
        /// 附件链接
        /// </summary>
        public string FilePath { get; set; }
        public string WebsiteOwner { get; set; }
    }
}