using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen
{
    /// <summary>
    /// 快递公司名称及代码
    /// </summary>
    public class ExpressInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressCompanyName { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        public string ExpressCompanyCode { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
