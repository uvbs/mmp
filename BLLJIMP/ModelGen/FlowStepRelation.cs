using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_FlowStepRelation
    /// </summary>
    [Serializable]
    public partial class FlowStepRelation : ModelTable
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
        /// 环节ID
        /// </summary>
        public int StepID { get; set; }
        /// <summary>
        /// 关系类型
        ///处理人	HandleUsers	
        ///处理角色	HandleGroups	
        ///开始通知角色	StartNoticeGroups	
        ///开始通知人	StartNoticeUsers	
        ///完成通知角色	EndNoticeGroups	
        ///完成通知人	EndNoticeUsers	
        /// </summary>
        public string RelationType { get; set; }
        /// <summary>
        /// 关系ID
        /// </summary>
        public string RelationID { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}