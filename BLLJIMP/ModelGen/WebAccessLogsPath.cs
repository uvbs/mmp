using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 网页日志 只记录在此表中的页面
    /// </summary>
    public class WebAccessLogsPath : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 页面路径 /web/a.aspx
        /// </summary>
        public string PagePath { get; set; }
    }
}
