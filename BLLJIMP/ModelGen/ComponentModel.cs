using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 模板
    /// </summary>
    [Serializable]
    public partial class ComponentModel : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 组件模板Key
        /// </summary>
        public string ComponentModelKey { get; set; }
        /// <summary>
        /// 组件模板名称
        /// </summary>
        public string ComponentModelName { get; set; }
        /// <summary>
        /// 组件模板分类
        /// </summary>
        public string ComponentModelType { get; set; }
        /// <summary>
        /// 组件访问链接
        /// {component_id}标识当前使用本模板的组件id
        /// </summary>
        public string ComponentModelLinkUrl { get; set; }
        /// <summary>
        /// 组件模板页面链接
        /// </summary>
        public string ComponentModelHtmlUrl { get; set; }
        /// <summary>
        /// 是否作废 0正常 1作废
        /// </summary>
        public int IsDelete { get; set; }
        
    }
}