using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信加V关键字映射
    /// </summary>
    public class WXAddVMaping : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string AddVKey { get; set; }
        /// <summary>
        /// 映射名称
        /// </summary>
        public string AddVValue { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }

}
