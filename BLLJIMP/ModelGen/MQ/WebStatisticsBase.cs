using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.MQ
{
    /// <summary>
    /// 网页统计基类
    /// </summary>
    public class WebStatisticsBase
    {
        public string EventUserID { get; set; }

        public string CurrfilePath { get; set; }

        public string SpreadUserId { get; set; }
        
        public string EventBrowser { get; set; }

        public string EventBrowserID { get; set; }

        public string EventBrowserIsBata { get; set; }

        public string EventBrowserVersion { get; set; }

        public string EventSysByte { get; set; }

        public string EventSysPlatform { get; set; }

        public string SourceIP { get; set; }

        public string IPLocation { get; set; }

        public string SourceUrl { get; set; }

        public string RequesSourcetUrl { get; set; }
        
        public int EventType { get; set; }

        public string ModuleType { get; set; }

        public string WebsiteOwner { get; set; }

        public DateTime EventDate { get; set; }

        public MonitorEventDetailsInfo GetMonitorEventDetailsInfo()
        {
            MonitorEventDetailsInfo data = new MonitorEventDetailsInfo();

            data.EventUserID = EventUserID;
            data.SpreadUserID = SpreadUserId;
            data.EventBrowser = EventBrowser;
            data.EventBrowserID = EventBrowserID;
            data.EventBrowserIsBata = EventBrowserIsBata;
            data.EventBrowserVersion = EventBrowserVersion;
            data.EventSysByte = EventSysByte;
            data.EventSysPlatform = EventSysPlatform;
            data.SourceIP = SourceIP;
            data.IPLocation = IPLocation;
            data.SourceUrl = SourceUrl;
            data.RequesSourcetUrl = RequesSourcetUrl;
            data.EventType = EventType;
            data.ModuleType = ModuleType;
            data.WebsiteOwner = WebsiteOwner;
            data.EventDate = EventDate;

            return data;
        }

    }
}
