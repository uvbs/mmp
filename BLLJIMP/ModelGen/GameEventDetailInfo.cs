using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class GameEventDetailInfo : ZCBLLEngine.ModelTable
    {
        #region Model

        /// <summary>
        /// 事件ID
        /// </summary>
        public int AutoID
        {
            get;
            set;
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int GamePlanID
        {
            get;
            set;
        }
        /// <summary>
        /// 来源IP
        /// </summary>
        public string SourceIP
        {
            get;
            set;
        }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string SourceUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime EventDate
        {
            get;
            set;
        }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string EventBrowser
        {
            get;
            set;
        }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string EventBrowserUserAgent
        {
            get;
            set;
        }

        /// <summary>
        /// 平台
        /// </summary>
        public string EventSysPlatform
        {
            get;
            set;
        }

        /// <summary>
        /// IP所在地
        /// </summary>
        public string IPLocation
        {
            get;
            set;
        }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }



        #endregion Model

    }
}
