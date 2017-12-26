using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.WXShow.WAP
{
    public partial class WXWAPShowInfo : System.Web.UI.Page
    {
        private BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public int Id;
        public StringBuilder strInit = new StringBuilder();
        public BLLJIMP.Model.WXShowInfo wxsInfo;
        public StringBuilder stranimation = new StringBuilder();
        /// <summary>
        /// 分享链接
        /// </summary>
        public string shareLink;
        protected void Page_Load(object sender, EventArgs e)
        {
            
             Id = Convert.ToInt32(Request["AutoId"]);
             shareLink = string.Format("http://{0}{1}?AutoId={2}", Request.Url.Host, Request.FilePath,Id);
             wxsInfo = bll.Get<BLLJIMP.Model.WXShowInfo>(" AutoId=" + Id);
             if (wxsInfo != null)
             {
                 string sourceimg = wxsInfo.ShowImg;
                 wxsInfo.ShowImg = "http://" + Request.Url.Host + wxsInfo.ShowImg;
                 List<BLLJIMP.Model.WXShowImgInfo> wxsInfos = bll.GetList<BLLJIMP.Model.WXShowImgInfo>(" ShowId=" + wxsInfo.AutoId);
                 if (wxsInfos != null)
                 {
                     for (int i = 0; i < wxsInfos.Count; i++)
                     {
                         strInit.AppendFormat("<div class=\"listli\"><img src=\"{0}\" class=\"img\" data-original=\"background-image:url({0}); \"></span>", wxsInfos[i].ImgStr);
                         strInit.AppendFormat("<span class=\"text\" style=\"color:{3};\"><h2 style=\"color:{2};\">{0}</h2>{1}</span>", wxsInfos[i].ShowTitle, wxsInfos[i].ShowContext, wxsInfos[i].ShowTitleColor, wxsInfos[i].ShowContextColor);
                         strInit.AppendFormat("<span class=\"nextbtn\"><span class=\"smallicon\"></span></span></div>");

                         if (string.IsNullOrEmpty(wxsInfos[i].ShowTitle) && string.IsNullOrEmpty(wxsInfos[i].ShowContext))
                         {
                             stranimation.Append("case " + i + ":_this.animation(current.find(\".img\"), " + wxsInfos[i].ShowAnimation.ToString() + ", function () {");
                             stranimation.Append(" _this.animation(current.find(\".nextbtn\"), 99);");
                             stranimation.Append("});break;");

                             stranimation.Append("\n");
                         }
                         else
                         {
                             //stranimation.AppendFormat("case " + i + ":_this.animation(current.find(\".img\"), " + wxsInfos[i].ShowAnimation + ", function () {");
                             string str = "case " + i + ":";
                             stranimation.Append(str + "_this.animation(current.find(\".img\")," + wxsInfos[i].ShowAnimation + ", function () {");
                             stranimation.Append("_this.animation(current.find(\".text\"), 2,");
                             string s = "function () { ";
                             stranimation.Append(s + "_this.animation(current.find(\".nextbtn\"), 99)});");
                             stranimation.Append("})");
                             stranimation.Append(";break;");

                             stranimation.Append("\n");
                         }
                     }
                 }
                 if (!string.IsNullOrEmpty(wxsInfo.ShowUrl))
                 {
                     stranimation.Append("case " + wxsInfos.Count + ":_this.animation(current.find(\".blackpage\"), 15, function (){ ");
                     stranimation.Append("_this.maininitstate++;if(_this.maininitstate");
                     stranimation.Append(" == 2) { _this.container.css({ \"-webkit-transition\"");
                     stranimation.Append(": \"opacity 1s ease-out\", \"opacity\": \"0\" });");

                     stranimation.AppendFormat("window.location.href = '{0}';", wxsInfo.ShowUrl);

                     stranimation.Append(" var picanimate1end = function () { _this.container[0].removeEventListener(\"webkitTransitionEnd\", picanimate1end, false);");
                     stranimation.Append("_this.container.remove();}; _this.container[0].addEventListener(\"webkitTransitionEnd\", picanimate1end, false);");
                     stranimation.Append(" }}); break;");

                     stranimation.Append("\n");
                 }
                 stranimation.Append("default ");
                 stranimation.Append(": _this.animation(current.find(\".img\")");
                 stranimation.Append(", 1, function () {");
                 stranimation.Append("_this.animation(current.find(\".text\"), 2,");
                 stranimation.Append("function () { _this.animation(current.find(\".nextbtn\"), 99)});");
                 stranimation.Append("});");
                

                 MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                 detailInfo.MonitorPlanID = Id;
                 detailInfo.EventType = 0;
                 detailInfo.EventBrowser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
                 detailInfo.EventBrowserID = HttpContext.Current.Request.Browser.Id; ;
                 if (HttpContext.Current.Request.Browser.Beta)
                     detailInfo.EventBrowserIsBata = "测试版";
                 else
                     detailInfo.EventBrowserIsBata = "正式版";

                 detailInfo.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;
                 detailInfo.EventDate = DateTime.Now;
                 if (HttpContext.Current.Request.Browser.Win16)
                     detailInfo.EventSysByte = "16位系统";
                 else
                     if (HttpContext.Current.Request.Browser.Win32)
                         detailInfo.EventSysByte = "32位系统";
                     else
                         detailInfo.EventSysByte = "64位系统";
                 detailInfo.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
                 detailInfo.SourceIP = Common.MySpider.GetClientIP();
                 detailInfo.IPLocation = Common.MySpider.GetIPLocation(detailInfo.SourceIP);
                 detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
                 detailInfo.RequesSourcetUrl = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() : "";
                 detailInfo.WebsiteOwner = bll.WebsiteOwner;
                 detailInfo.ModuleType = "wshow";
                 if (bll.IsLogin)
                 {
                     detailInfo.EventUserID = bll.GetCurrUserID();
                 }
                 bll.Add(detailInfo);

                 int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ",bll.WebsiteOwner,Id));
                 int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, Id));
                 int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ",bll.WebsiteOwner,Id));

                 wxsInfo.PV = pvCount;
                 wxsInfo.IP = ipCount;
                 wxsInfo.UV = uvCount;
                 wxsInfo.ShowImg = sourceimg;
                 bll.Update(wxsInfo);


                 if (!string.IsNullOrEmpty(Request["sid"]))//有推广人
                 {
                     if (!string.IsNullOrEmpty(wxsInfo.ShowUrl))
                     {
                         if (wxsInfo.ShowUrl.EndsWith(".chtml"))
                         {

                             //替换链接
                             var par = wxsInfo.ShowUrl.Split('/');
                             wxsInfo.ShowUrl = wxsInfo.ShowUrl.Replace(par[3], string.Format("{0}/{1}", par[3], Request["sid"]));
                             shareLink = string.Format("{0}&sid={1}", shareLink, Request["sid"]);


                         }
                     }

                 }
                 else
                 {
                     if (bll.IsLogin)
                     {
                         if (!string.IsNullOrEmpty(wxsInfo.ShowUrl))
                         {
                             if (wxsInfo.ShowUrl.EndsWith(".chtml"))
                             {
                                 UserInfo CurrentUserInfo = bll.GetCurrentUserInfo();
                                 string CurrentUserIDHex = Convert.ToString(CurrentUserInfo.AutoID, 16);
                                 //替换链接
                                 var par = wxsInfo.ShowUrl.Split('/');
                                 wxsInfo.ShowUrl = wxsInfo.ShowUrl.Replace(par[3], string.Format("{0}/{1}", par[3], CurrentUserIDHex));
                                 shareLink = string.Format("{0}&sid={1}", shareLink, CurrentUserIDHex);

                             }
                         }


                     }

                 }


             }
            
        }



        
    }
}