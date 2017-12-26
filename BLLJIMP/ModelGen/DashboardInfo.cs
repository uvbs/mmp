using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_DashboardInfo
    /// </summary>
    [Serializable]
    public partial class DashboardInfo : ModelTable
    {
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 最后生成日期
        /// </summary>
        public int Date { get; set; }
        /// <summary>
        /// 展示Json
        /// </summary>
        public string Json { get; set; }
    }
}