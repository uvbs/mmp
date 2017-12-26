using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using ZentCloud.BLLJIMP;

namespace ZentCloud.BLLModule
{
    public class ShareReaderModule : IHttpModule
    {
        void IHttpModule.Dispose()
        {

        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        private void context_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {

                ToLog("进入ShareReaderModule");

                HttpApplication application = (HttpApplication)sender;

                var currUrl = application.Request.Url.ToString().ToLower();

                #region 做一些其他过滤
                var currentPath = application.Request.FilePath == null ? "" : application.Request.FilePath.ToLower();

                string pageExtraName = System.IO.Path.GetExtension(currentPath);

                List<string> pageExtraNameFilterList = new List<string>()
                {
                    ".aspx",
                    //".ashx",
                    //".cn",
                    //".com",
                    //".net",
                    ".chtml"
                };

                if (!pageExtraNameFilterList.Contains(pageExtraName))
                {
                    return;
                }

                #endregion

                BLLShareMonitor bllShareMonitor = new BLLShareMonitor();

                //获取分享id
                var shareId = application.Request["comeonshareid"];

                int monitorId = 0;

                if (string.IsNullOrWhiteSpace(shareId))
                {
                    ToLog("没有shareid");
                    monitorId = bllShareMonitor.GetMonitorByUrl(currUrl).MonitorId;

                    if (monitorId == 0)
                    {
                        ToLog("没有monitorId");
                        return;
                    }
                    ToLog("有monitorId");
                }
                else
                {

                }

                ToLog("进一步记录");

                string userID = string.Empty;
                UserInfo userModel = null;

                if (application.Session == null || application.Session[Common.SessionKey.LoginStatu] == null || application.Session[Common.SessionKey.UserID] == null)
                {
                    //TODO:未登录
                }
                else
                {
                    //获取用户信息
                    userID = application.Session[Common.SessionKey.UserID].ToString();//获取登录ID
                    userModel = new ZentCloud.BLLJIMP.BLLUser("").GetUserInfo(userID);
                }

                ShareReaderInfo model = new ShareReaderInfo()
                {
                    IP = ZentCloud.Common.MySpider.GetClientIP(),
                    IPLocation = ZentCloud.Common.MySpider.GetIPLocation(ZentCloud.Common.MySpider.GetClientIP()),
                    ReadTime = DateTime.Now,
                    ShareId = shareId,
                    Url = currUrl,
                    MonitorId = monitorId
                };

                if (userModel != null)
                {
                    model.UserId = userModel.UserID;
                    model.UserWxOpenId = userModel.WXOpenId;
                }

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
                }

                var addResult = BLLJIMP.BLLStatic.bll.Add(model);
            }
            catch (Exception ex)
            {
                ToLog("异常：" + ex.Message);
            }

        }


        private void ToLog(string msg)
        {
            try
            {
                //using (StreamWriter sw = new StreamWriter(@"D:\ShareReaderModule.txt", true, Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                //}
            }
            catch { }
        }


    }
}
