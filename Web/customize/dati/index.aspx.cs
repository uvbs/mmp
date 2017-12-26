using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.dati
{
    public partial class index : System.Web.UI.Page
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        BLLUser bllUser = new BLLUser();
        UserInfo curUser;
        bool isLogin;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string indexStr = File.ReadAllText(this.Server.MapPath("index.html"));
                string AutoID = this.Request["id"];
                isLogin = bllUser.IsLogin;
                BLLJIMP.Model.QuestionnaireSet QuestionnaireSetModel = bllQuestion.GetByKey<BLLJIMP.Model.QuestionnaireSet>("AutoID", AutoID);
                if (QuestionnaireSetModel != null && isLogin)
                {
                    curUser = bllUser.GetCurrentUserInfo();
                    JToken set = JToken.FromObject(QuestionnaireSetModel);
                    set["IsLogin"] = isLogin;
                    set["UserScoreNum"] = bllUser.GetUserScoreNum(curUser.UserID, "QuestionnaireSet", false, QuestionnaireSetModel.AutoID.ToString());
                    indexStr = indexStr.Replace("{AutoID:0}", JsonConvert.SerializeObject(set));
                }
                else
                {
                    indexStr = indexStr.Replace("{AutoID:0}", "{AutoID:0,IsLogin:" + isLogin + "}");
                }

                MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                detailInfo.MonitorPlanID = int.Parse(AutoID);
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

                detailInfo.WebsiteOwner = bllUser.WebsiteOwner;
                detailInfo.ModuleType = "questionnaireset";
                if (bllUser.IsLogin)
                {
                    detailInfo.EventUserID = bllUser.GetCurrUserID();
                }
                detailInfo.ShareTimestamp = "2";
                bllUser.Add(detailInfo);

                int ipCount = bllUser.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='2' ", bllUser.WebsiteOwner, int.Parse(AutoID)));
                int uvCount = bllUser.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='2' ", bllUser.WebsiteOwner, AutoID));
                int pvCount = bllUser.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='2' ", bllUser.WebsiteOwner, int.Parse(AutoID)));

                QuestionnaireSetModel.IP = ipCount;
                QuestionnaireSetModel.PV = pvCount;
                QuestionnaireSetModel.UV = uvCount;
                bllUser.Update(QuestionnaireSetModel);
                this.Response.Write(indexStr);
            }
           
        }
    }
}