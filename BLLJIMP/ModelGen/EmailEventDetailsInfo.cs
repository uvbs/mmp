using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 检测统计明细表
	/// </summary>
	[Serializable]
    public partial class EmailEventDetailsInfo : ZCBLLEngine.ModelTable
	{
		public EmailEventDetailsInfo()
		{}
        #region Model
        private int _autoid;
        private string _emailid;
        private int _eventtype;
        private string _sourseip;
        private string _sourseurl;
        private DateTime _eventdate;
        private string _eventemail;
        private string _eventbrowser;
        private string _eventbrowserid;
        private string _eventbrowserversion;
        private string _eventbrowserisbata;
        private string _eventsysplatform;
        private string _eventsysbyte;
        private string _linkid;
        /// <summary>
        /// 统计ID
        /// </summary>
        public int AutoId
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 邮件ID
        /// </summary>
        public string EmailID
        {
            set { _emailid = value; }
            get { return _emailid; }
        }
        /// <summary>
        /// 触发类型
        /// </summary>
        public int EventType
        {
            set { _eventtype = value; }
            get { return _eventtype; }
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
        /// 触发邮箱
        /// </summary>
        public string EventEmail
        {
            set { _eventemail = value; }
            get { return _eventemail; }
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
        /// 
        /// </summary>
        public string LinkID
        {
            set { _linkid = value; }
            get { return _linkid; }
        }
        #endregion Model
	}
}

