using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class MonitorEventDetailsInfo : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 点击URL
        /// </summary>
        public string ClickUrl
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// 访问次数
        /// </summary>
        public int tCount { get; set; }
      
 

    }
}
