using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信加V分类
    /// </summary>
    public class WXAddVCategory : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryKey { get; set; }
        /// <summary>
        /// 分类值名称 对应目录名称
        /// </summary>
        public string CategoryValue { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }

}
