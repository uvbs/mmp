using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_Flow
    /// </summary>
    [Serializable]
    public partial class Flow : ModelTable
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// FlowKey
        /// </summary>
        public string FlowKey { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}