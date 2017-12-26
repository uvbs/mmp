using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Serializable]
    public class UserLoginLogs : ZCBLLEngine.ModelTable
    {
        #region Model
        private long _autoid;
        private string _userid;
        private string _ip;
        private string _iplocation;
        private DateTime _insertdate;
        private string _browser;
        private string _browserid;
        private string _browserversion;
        private string _browserisbata;
        private string _systemplatform;
        private string _systembyte;
        /// <summary>
        /// 自增ID
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
        /// 登录时间
        /// </summary>
        public DateTime InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser
        {
            set { _browser = value; }
            get { return _browser; }
        }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string BrowserID
        {
            set { _browserid = value; }
            get { return _browserid; }
        }
        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string BrowserVersion
        {
            set { _browserversion = value; }
            get { return _browserversion; }
        }
        /// <summary>
        /// 浏览器是否测试版
        /// </summary>
        public string BrowserIsBata
        {
            set { _browserisbata = value; }
            get { return _browserisbata; }
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string SystemPlatform
        {
            set { _systemplatform = value; }
            get { return _systemplatform; }
        }
        /// <summary>
        /// 系统位数
        /// </summary>
        public string SystemByte
        {
            set { _systembyte = value; }
            get { return _systembyte; }
        }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        #endregion Model
    }
}
