using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 访问日志表
    /// </summary>
    [Serializable]
    public partial class WebAccessLogsInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public WebAccessLogsInfo()
        { }

        #region Model
        private long _autoid;
        private string _userid;
        private string _pageurl;
        private DateTime _accessdate = DateTime.Now;
        private string _ip;
        private string _iplocation;
        private string _eventbrowser;
        private string _eventbrowserid;
        private string _eventbrowserversion;
        private string _eventbrowserisbata;
        private string _eventsysplatform;
        private string _eventsysbyte;
        /// <summary>
        /// 自动编号
        /// </summary>
        public long AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 页面路径
        /// </summary>
        public string PageUrl
        {
            set { _pageurl = value; }
            get { return _pageurl; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime AccessDate
        {
            set { _accessdate = value; }
            get { return _accessdate; }
        }
        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// IP所在地
        /// </summary>
        public string IPLocation
        {
            set { _iplocation = value; }
            get { return _iplocation; }
        }
        /// <summary>
        /// 浏览器
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
        /// 浏览器版本
        /// </summary>
        public string EventBrowserVersion
        {
            set { _eventbrowserversion = value; }
            get { return _eventbrowserversion; }
        }
        /// <summary>
        /// 浏览器是否是测试版
        /// </summary>
        public string EventBrowserIsBata
        {
            set { _eventbrowserisbata = value; }
            get { return _eventbrowserisbata; }
        }
        /// <summary>
        /// 系统平台 windows maos
        /// </summary>
        public string EventSysPlatform
        {
            set { _eventsysplatform = value; }
            get { return _eventsysplatform; }
        }
        /// <summary>
        /// 系统位数 32 或 64
        /// </summary>
        public string EventSysByte
        {
            set { _eventsysbyte = value; }
            get { return _eventsysbyte; }
        }
        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string WXOpenID { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string EventUserAgent { get; set; }
        /// <summary>
        /// 是否是移动设备访问
        /// </summary>
        public string EventIsMobileDevice { get; set; }

        public string EventMobileDeviceManufacturer { get; set; }
        /// <summary>
        /// 移动设备型号
        /// </summary>
        public string EventMobileDeviceModel { get; set; }
        /// <summary>
        /// 扩展
        /// </summary>
        public string Ex_ID { get; set; }
        /// <summary>
        /// 分享人用户名
        /// </summary>
        public string Ex_SpreadUserID { get; set; }
        /// <summary>
        /// 分享时间戳
        /// </summary>
        public string Ex_ShareTimestamp { get; set; }
        /// <summary>
        /// 上一个分享人用户名
        /// </summary>
        public string Ex_PreSpreadUserID { get; set; }
        /// <summary>
        /// 上一个分享人分享时间戳
        /// </summary>
        public string Ex_PreShareTimestamp { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        #endregion Model

    }
}
