using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace BLLWebAccessLogsModule
{
    /// <summary>
    /// 
    /// </summary>
    public class BLL : ZentCloud.ZCBLLEngine.BLLBase
    {


        protected override string GetRealTableName(string modelName)
        {
            string tableName = modelName.EndsWith("Ex", true, null) ? modelName.Substring(0, modelName.Length - 2) : modelName;

            return "ZCJ_" + tableName;
        }
    }

    /// <summary>
    /// 
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
        /// 
        /// </summary>
        public long AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PageUrl
        {
            set { _pageurl = value; }
            get { return _pageurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AccessDate
        {
            set { _accessdate = value; }
            get { return _accessdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IPLocation
        {
            set { _iplocation = value; }
            get { return _iplocation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventBrowser
        {
            set { _eventbrowser = value; }
            get { return _eventbrowser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventBrowserID
        {
            set { _eventbrowserid = value; }
            get { return _eventbrowserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventBrowserVersion
        {
            set { _eventbrowserversion = value; }
            get { return _eventbrowserversion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventBrowserIsBata
        {
            set { _eventbrowserisbata = value; }
            get { return _eventbrowserisbata; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventSysPlatform
        {
            set { _eventsysplatform = value; }
            get { return _eventsysplatform; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventSysByte
        {
            set { _eventsysbyte = value; }
            get { return _eventsysbyte; }
        }

        public string WXOpenID { get; set; }

        public string EventUserAgent { get; set; }

        public string EventIsMobileDevice { get; set; }

        public string EventMobileDeviceManufacturer { get; set; }

        public string EventMobileDeviceModel { get; set; }

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

        public string WebsiteOwner { get; set; }

        #endregion Model

    }


    /// <summary>
    /// 网页访问日志处理模块
    /// </summary>
    public class WebAccessLogsModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            //记录每次访问页面路径(aspx、chtml、ashx、htm、html)、用户ID(没有则为空)、访问时间、IP、浏览器相关信息
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            WriteWebAccessLogs(application);
        }

        public void WriteWebAccessLogs(HttpApplication application)
        {

            try
            {
                string accessUrl = application.Request.Url.ToString();
                string currAbsolutePath = application.Request.Url.AbsolutePath == null ? "" : application.Request.Url.AbsolutePath.ToLower();

                string pageExtraName = ZentCloud.Common.IOHelper.GetExtraName(currAbsolutePath);

                List<string> pageExtraNameFilterList = new List<string>()
                {
                    "aspx","chtml"
                };

                //只处理aspx、chtml页面
                if (!pageExtraNameFilterList.Contains(pageExtraName))
                    return;
                ToLog("进入 WriteWebAccessLogs");

                BLL bll = new BLL();

                //List<WebAccessLogsPath> pathList = bll.GetList<WebAccessLogsPath>("");
                //ToLog("判断是否跳过 currUrl:" + accessUrl + "  pathList:" + JsonConvert.SerializeObject(pathList));
                //if ((pathList.Where(p => p.PagePath.ToLower().Equals(application.Request.FilePath.ToLower())).ToList().Count <= 0) && (!pageExtraName.Equals("chtml")))
                //    return;

                //ToLog("没有跳过");
                

                //userid openid
                string userId = "";// application.Session["userID"] == null ? "" : application.Session["userID"].ToString();
                string wxOpenId = "";// application.Session["WXCurrOpenerOpenID"] == null ? "" : application.Session["WXCurrOpenerOpenID"].ToString();//WXCurrOpenerOpenID

                try
                {
                    userId = application.Session["userID"] == null ? "" : application.Session["userID"].ToString();
                }
                catch { }

                try
                {
                    wxOpenId = application.Session["WXCurrOpenerOpenID"] == null ? "" : application.Session["WXCurrOpenerOpenID"].ToString();//WXCurrOpenerOpenID
                }
                catch { }

                ToLog(string.Format("取到的 wxOpenId {0}  userid {1} ", wxOpenId, userId));

                WebAccessLogsInfo model = new WebAccessLogsInfo()
                {
                    UserID = userId,
                    WXOpenID = wxOpenId,
                    PageUrl = accessUrl,
                    AccessDate = DateTime.Now,
                    IP = ZentCloud.Common.MySpider.GetClientIP()
                };
                ToLog("正在处理0");

                model.IPLocation = ZentCloud.Common.MySpider.GetIPLocation(ZentCloud.Common.MySpider.GetClientIP());

                model.EventUserAgent = application.Request.UserAgent;

                if (application.Request.Browser != null)
                {
                    model.EventBrowser = application.Request.Browser.Browser;
                    model.EventBrowserID = application.Request.Browser.Id;
                    model.EventBrowserIsBata = application.Request.Browser.Beta.ToString();
                    model.EventBrowserVersion = application.Request.Browser.Version;
                    model.EventSysPlatform = application.Request.Browser.Platform;
                    if (application.Request.Browser.Win16)
                        model.EventSysByte = "16";
                    else if (application.Request.Browser.Win32)
                        model.EventSysByte = "32";
                    else
                        model.EventSysByte = "64";
                    model.EventIsMobileDevice = application.Request.Browser.IsMobileDevice.ToString();
                    model.EventMobileDeviceManufacturer = application.Request.Browser.MobileDeviceManufacturer;
                    model.EventMobileDeviceModel = application.Request.Browser.MobileDeviceModel;
                }

                ToLog("正在处理1");

                string exId = application.Request["ID"];
                string exSpreadUserId = application.Request["SpreadU"];
                string exShareTimestamp = application.Request["ShareTimestamp"];


                string exPreSpreadUserId = application.Request["PreSpreadU"];
                string exPreShareTimestamp = application.Request["PreShareTimestamp"];


                if (!string.IsNullOrWhiteSpace(exSpreadUserId))
                {
                    exSpreadUserId = ZentCloud.Common.Base64Change.DecodeBase64ByUTF8(exSpreadUserId);
                }
                if (!string.IsNullOrWhiteSpace(exPreSpreadUserId))
                {
                    exPreSpreadUserId = ZentCloud.Common.Base64Change.DecodeBase64ByUTF8(exPreSpreadUserId);
                }

                ToLog("正在处理2");

                model.Ex_ID = exId;
                model.Ex_SpreadUserID = exSpreadUserId;
                model.Ex_ShareTimestamp = exShareTimestamp;

                model.Ex_PreSpreadUserID = exPreSpreadUserId;
                model.Ex_PreShareTimestamp = exPreShareTimestamp;

                ToLog("正在处理3");

                ZentCloud.BLLJIMP.Model.WebsiteInfo websiteInfo = new ZentCloud.BLLJIMP.Model.WebsiteInfo();
                if (System.Web.HttpContext.Current.Session["WebsiteInfoModel"] != null)
                {
                    websiteInfo = (ZentCloud.BLLJIMP.Model.WebsiteInfo)System.Web.HttpContext.Current.Session["WebsiteInfoModel"];
                }

                ToLog("正在处理4");

                model.WebsiteOwner = websiteInfo.WebsiteOwner;
                ToLog("正在处理5");

                bll.Add(model);
                ToLog("正在处理6");
                //ToLog(string.Format("WriteWebAccessLogs end wxOpenId {0}  userid {1} ", wxOpenId, userId));
            }
            catch (Exception ex)
            {
                ToLog("WebAccessLogsModule异常:" + ex.Message);
            }
        }



        private void ToLog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\log\WriteWebAccess.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                }
            }
            catch { }
        }

        //private void ToLog(string msg, HttpApplication application)
        //{
        //    try
        //    {
        //        string path = application.Server.MapPath("/FileUpload/logs.txt");

        //        using (StreamWriter sw = new StreamWriter(path, true, Encoding.GetEncoding("gb2312")))
        //        {
        //            sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
        //        }
        //    }
        //    catch { }
        //}
    }
}
