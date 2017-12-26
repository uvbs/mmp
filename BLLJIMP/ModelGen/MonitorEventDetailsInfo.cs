using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 监测事件记录
    /// </summary>
    [Serializable]
    public partial class MonitorEventDetailsInfo : ZCBLLEngine.ModelTable
    {
        #region Model
        private int? _detailid;
        private int _monitorplanid;
        private int? _eventtype;
        private string _sourceip;
        private string _sourceurl;
        private DateTime? _eventdate;
        private string _eventbrowser;
        private string _eventbrowserid;
        private string _eventbrowserversion;
        private string _eventbrowserisbata;
        private string _eventsysplatform;
        private string _eventsysbyte;
        private int _linkid;
        private string _iplocation;
        /// <summary>
        /// 事件ID
        /// </summary>
        public int? DetailID
        {
            set { _detailid = value; }
            get { return _detailid; }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int MonitorPlanID
        {
            set { _monitorplanid = value; }
            get { return _monitorplanid; }
        }
        /// <summary>
        /// 事件类型 0代表打开 1代表点击
        /// </summary>
        public int? EventType
        {
            set { _eventtype = value; }
            get { return _eventtype; }
        }
        /// <summary>
        /// 来源IP
        /// </summary>
        public string SourceIP
        {
            set { _sourceip = value; }
            get { return _sourceip; }
        }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string SourceUrl
        {
            set { _sourceurl = value; }
            get { return _sourceurl; }
        }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime? EventDate
        {
            set { _eventdate = value; }
            get { return _eventdate; }
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
        /// 浏览器名称
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
        /// 浏览器是否测试版
        /// </summary>
        public string EventBrowserIsBata
        {
            set { _eventbrowserisbata = value; }
            get { return _eventbrowserisbata; }
        }
        /// <summary>
        /// 平台
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
        /// 链接ID
        /// </summary>
        public int LinkID
        {
            set { _linkid = value; }
            get { return _linkid; }
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
        /// 触发的用户ID
        /// </summary>
        public string EventUserID { get; set; }

        /// <summary>
        /// 推广的用户ID
        /// </summary>
        public string SpreadUserID { get; set; }

        /// <summary>
        /// 推广人自增ID
        /// </summary>
        public int SpreadUserAutoID { get; set; }

        /// <summary>
        /// 分享时间戳
        /// </summary>
        public string ShareTimestamp { get; set; }

        /// <summary>
        /// 模块类型
        /// product  商品
        /// shake    摇一摇
        /// scratch  刮刮奖 
        /// article  普通文章
        /// activity 活动
        /// greetingcard 贺卡
        /// wshow    微秀
        /// question   问卷
        /// questionnaireset 答题
        /// thevote  选题投票
        /// </summary>
        public string ModuleType { get; set; }

        #endregion Model

        #region ModelEx
        /// <summary>
        /// 推广人显示名称（目前显示手机号）
        /// </summary>
        public string SpreadUserShowName
        {
            get
            {
                //try
                //{
                //    UserInfo user = new BLLUser("").GetUserInfoByAutoID(SpreadUserAutoID);
                //    if (user == null)
                //        return "";
                //    return user.Phone;
                //}
                //catch
                //{
                //    return "";
                //}
                return "";
            }
        }


        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 触发用户微信头像
        /// </summary>
        public string EventUserWXImg { get; set; }
        /// <summary>
        /// 触发用户微信昵称
        /// </summary>
        public string EventUserWXNikeName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string EventUserTrueName { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string EventUserPhone { get; set; }


        public string ModuleTyppeString
        {
            get {
                switch (ModuleType)
                {
                    case "article":
                        return "文章";
                    case "activity":
                        return "活动";
                    case "product":
                        return "商品";
                    case "question":
                        return "问卷";
                    case "questionnaireset":
                        return "答题";
                    case "thevote":
                        return "选题投票";
                    case "greetingcard":
                        return "贺卡";
                    case "scratch":
                        return "刮刮奖";
                    case "shake":
                        return "摇一摇";
                    case "wshow":
                        return "微秀";
                    default:
                        return "其它";
                }
            }
        }

        /// <summary>
        /// 请求的来源地址
        /// </summary>
        public string RequesSourcetUrl { get; set; }
        #endregion

    }
}
