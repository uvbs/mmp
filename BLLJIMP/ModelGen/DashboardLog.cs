using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_DashboardLog
    /// </summary>
    [Serializable]
    public partial class DashboardLog : ModelTable
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string DashboardType { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }
}