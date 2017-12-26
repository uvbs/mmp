using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户模板文件
    /// </summary>
    public class UserTemplate : ZentCloud.ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public string TemplateType { get; set; }
        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
