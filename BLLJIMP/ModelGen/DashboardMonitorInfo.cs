using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public partial class DashboardMonitorInfo : ModelTable
    {	
        /// <summary>
        /// DetailID
        /// </summary>
        public int DetailID { get; set; }
        /// <summary>
        /// SourceIP
        /// </summary>
        public string SourceIP { get; set; }
        /// <summary>
        /// IPLocation
        /// </summary>
        public string IPLocation { get; set; }
        /// <summary>
        /// EventDate
        /// </summary>
        public DateTime EventDate { get; set; }
        /// <summary>
        /// EventBrowserID
        /// </summary>
        public string EventBrowserID { get; set; }
        /// <summary>
        /// EventUserID
        /// </summary>
        public string EventUserID { get; set; }
        /// <summary>
        /// WebsiteOwner
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// EventUserWXImg
        /// </summary>
        public string EventUserWXImg { get; set; }
        /// <summary>
        /// EventUserWXNikeName
        /// </summary>
        public string EventUserWXNikeName { get; set; }
        /// <summary>
        /// EventUserTrueName
        /// </summary>
        public string EventUserTrueName { get; set; }
        /// <summary>
        /// EventUserPhone
        /// </summary>
        public string EventUserPhone { get; set; }
    }
}
