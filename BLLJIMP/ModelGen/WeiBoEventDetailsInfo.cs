using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class WeiBoEventDetailsInfo : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _autoid;
        private string _sourseip;
        private string _sourseurl;
        private DateTime _eventdate;
        private string _eventbrowser;
        private string _eventbrowserid;
        private string _eventbrowserversion;
        private string _eventbrowserisbata;
        private string _eventsysplatform;
        private string _eventsysbyte;

        /// <summary>
        /// 统计ID
        /// </summary>
        public int AutoId
        {
            set { _autoid = value; }
            get { return _autoid; }
        }


        /// <summary>
        /// 来源IP
        /// </summary>
        public string SourseIP
        {
            set { _sourseip = value; }
            get { return _sourseip; }
        }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string SourseUrl
        {
            set { _sourseurl = value; }
            get { return _sourseurl; }
        }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime EventDate
        {
            set { _eventdate = value; }
            get { return _eventdate; }
        }
        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string EventBrowser
        {
            set { _eventbrowser = value; }
            get { return _eventbrowser; }
        }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string EventBrowserID
        {
            set { _eventbrowserid = value; }
            get { return _eventbrowserid; }
        }
        /// <summary>
        /// 浏览器版本号
        /// </summary>
        public string EventBrowserVersion
        {
            set { _eventbrowserversion = value; }
            get { return _eventbrowserversion; }
        }
        /// <summary>
        /// 是否测试版
        /// </summary>
        public string EventBrowserIsBata
        {
            set { _eventbrowserisbata = value; }
            get { return _eventbrowserisbata; }
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string EventSysPlatform
        {
            set { _eventsysplatform = value; }
            get { return _eventsysplatform; }
        }
        /// <summary>
        /// 系统位数
        /// </summary>
        public string EventSysByte
        {
            set { _eventsysbyte = value; }
            get { return _eventsysbyte; }
        }
       /// <summary>
       /// CID
       /// </summary>
        public string CID { get; set; }
        /// <summary>
        /// VIEWID
        /// </summary>
        public string ViewID { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// IP地点
        /// </summary>
        public string IPLocation { get; set; }
        #endregion Model
    }
}
