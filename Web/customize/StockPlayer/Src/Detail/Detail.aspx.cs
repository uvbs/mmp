using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Detail
{
    public partial class Detail : System.Web.UI.Page
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        ZentCloud.BLLJIMP.BLLCommRelation bLLCommRelation = new ZentCloud.BLLJIMP.BLLCommRelation();
        public BLLJIMP.Model.JuActivityInfo model = new BLLJIMP.Model.JuActivityInfo();
        public BLLJIMP.Model.UserInfo userInfo = new BLLJIMP.Model.UserInfo();
        public string userName = string.Empty;
        public string avatar = string.Empty;
        public string currUserName = string.Empty;
        public string currAvatar = string.Empty;
        public bool hasPraise = false;
        protected string sendNoticePrice;
        protected bool isFriend=false;
        protected void Page_Load(object sender, EventArgs e)
        {
            string jid=Request["jid"];
            if (!string.IsNullOrEmpty(jid))
            {
                model = bllJuActivity.GetJuActivity(int.Parse(jid),true,bllJuActivity.WebsiteOwner);

                if (model == null) {
                    model = new JuActivityInfo();
                    return; 
                }
                if (!string.IsNullOrEmpty(model.UserID))
                {
                    userInfo = bllUser.GetUserInfo(model.UserID);
                    userName = bllUser.GetUserDispalyName(userInfo);
                    avatar = bllUser.GetUserDispalyAvatar(userInfo);
                }
                if (model.ArticleType != "Comment")
                {
                    this.Title = model.ActivityName;
                }
                else
                {
                    this.Title = model.Summary;
                }
                BLLJIMP.Model.UserInfo curUser = bllUser.GetCurrentUserInfo();

                #region 事件记录 IP访问记录
                //事件
                //int OpenCount = 0;//打开人数
                //int DistinctOpenCount = 0;//独立IP数量
                MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                detailInfo.MonitorPlanID = model.MonitorPlanID;
                detailInfo.EventType = 0;
                detailInfo.EventBrowser = this.Request.Browser == null ? "" : this.Request.Browser.ToString();
                detailInfo.EventBrowserID = this.Request.Browser.Id; ;
                if (this.Request.Browser.Beta)
                {
                    detailInfo.EventBrowserIsBata = "测试版";
                }
                else
                {
                    detailInfo.EventBrowserIsBata = "正式版";
                }

                detailInfo.EventBrowserVersion = this.Request.Browser.Version;
                detailInfo.EventDate = DateTime.Now;
                if (this.Request.Browser.Win16)
                    detailInfo.EventSysByte = "16位系统";
                else
                    if (this.Request.Browser.Win32)
                        detailInfo.EventSysByte = "32位系统";
                    else
                        detailInfo.EventSysByte = "64位系统";

                detailInfo.EventSysPlatform = this.Request.Browser.Platform;
                detailInfo.SourceIP = Common.MySpider.GetClientIP();
                detailInfo.IPLocation = Common.MySpider.GetIPLocation(detailInfo.SourceIP);
                detailInfo.SourceUrl = this.Request.Url.ToString();
                detailInfo.RequesSourcetUrl = this.Request.UrlReferrer != null ? this.Request.UrlReferrer.ToString() : "";

                detailInfo.WebsiteOwner = bllJuActivity.WebsiteOwner;
                detailInfo.ModuleType = model.ArticleType;
                if (curUser != null)
                {
                    detailInfo.EventUserID = curUser.UserID;
                }
                bllJuActivity.Add(detailInfo);
                //更新IP数 PV数
                //阅读数加一
                model.PV++;
                bllJuActivity.UpdateIPCount(model);
                #endregion

                if (curUser != null){
                    currUserName = bllUser.GetUserDispalyName(curUser);
                    currAvatar = bllUser.GetUserDispalyAvatar(curUser);
                    #region 关系
                    hasPraise = bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.JuActivityPraise, jid, curUser.AutoID.ToString());
                    #endregion
                    #region 加积分
                    string tempMsg = "";
                    bllUser.AddUserScoreDetail(curUser.UserID, "ReadType", bllUser.WebsiteOwner, out tempMsg, null, null, model.ArticleType, true, model.ArticleType);
                    bllUser.AddUserScoreDetail(curUser.UserID, "ReadCategory", bllUser.WebsiteOwner, out tempMsg, null, null, model.CategoryId, true, model.CategoryId);
                    bllUser.AddUserScoreDetail(curUser.UserID, "ReadArticle", bllUser.WebsiteOwner, out tempMsg, null, "《" + model.ActivityName + "》", model.JuActivityID.ToString(), true, model.JuActivityID.ToString(), model.ArticleType);
                    #endregion
                }


                sendNoticePrice = bllKeyValueData.GetDataVaule("SendNoticePrice", "1", bllKeyValueData.WebsiteOwner);
                if (string.IsNullOrWhiteSpace(sendNoticePrice)) sendNoticePrice = "-1";

                if (model.ArticleType == "CompanyPublish" || (curUser!=null&&curUser.UserType==6))
                {
                    isFriend = true;
                }
                else if(curUser!=null)
                {
                    isFriend = bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.Friend, userInfo.AutoID.ToString(), curUser.AutoID.ToString());
                }
                
            }

        }
    }
}