using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.App.Questionnaire.wap
{
    public partial class Questionnaire : System.Web.UI.Page
    {
        public BLLJIMP.Model.Questionnaire QuestionnaireModel = new BLLJIMP.Model.Questionnaire();
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public UserInfo currUserInfo = new UserInfo();
        public UserInfo spreadUser = new UserInfo();
        public string uid = string.Empty;
        public bool isSubmit = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!bll.IsLogin)
            {
                Response.Write("您还未登录");
                Response.End();
                return;
            }
            string id = Request["id"];

            currUserInfo = bll.GetCurrentUserInfo();

            isSubmit = bllUser.GetCount<BLLJIMP.Model.QuestionnaireRecord>(string.Format("UserId='{0}' And QuestionnaireID={1}", currUserInfo.UserID, id)) > 0;

            //推广人
            uid = Request["uid"];
            spreadUser = bllUser.GetUserInfo(uid);
            string filePath = Request.Url.ToString();//当前相对路径
            
            int idInt = 0;
            if (string.IsNullOrEmpty(id))
            {
                Response.Write("无参数");
                Response.End();
                return;
            }
            if (!int.TryParse(id, out idInt))
            {
                Response.Write("无效参数");
                Response.End();
                return;
            }
            QuestionnaireModel = bll.Get<BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}", id));

            if (QuestionnaireModel == null)
            {
                Response.Write("问卷不存在");
                Response.End();
                return;
            }
            if (isSubmit)
            {
                if (!string.IsNullOrEmpty(QuestionnaireModel.QuestionnaireRepeatSubmitUrl))
                {
                    Response.Redirect(QuestionnaireModel.QuestionnaireRepeatSubmitUrl);
                }
               
            }
            if (QuestionnaireModel.QuestionnaireVisible.Equals(0))
            {
                Response.Write("问卷不显示");
                Response.End();
                return;
            }
            if (QuestionnaireModel.QuestionnaireStopDate != null)
            {
                if (DateTime.Now >= QuestionnaireModel.QuestionnaireStopDate)
                {
                    Response.Write("问卷已停止");
                    Response.End();
                    return;
                }
            }

            if (QuestionnaireModel.IsWeiXinLicensing == 1)
            {

                if (string.IsNullOrEmpty(currUserInfo.WXHeadimgurl))//无头像,重新高级授权
                {
                    Session.Clear();
                    Response.Redirect(string.Format("/customize/index.aspx?redirectUrl={0}", HttpUtility.UrlEncode(Request.Url.ToString())));

                }
            }

            StuViewCountParms sParms = new StuViewCountParms()
            {
                CurrfilePath = filePath,
                EventUserID = bll.IsLogin ? bll.GetCurrUserID() : "",
                QuestionnaireID = int.Parse(id),
                SpreadUser = spreadUser,
                HttpContextCurrent = HttpContext.Current,
                WebsiteOwner = bll.WebsiteOwner
            };
            
            Thread t = new Thread(new ThreadStart(sParms.StuViewCount));
            t.Start();
            
        }
        
        //开一个线程去处理计数
        
        public class StuViewCountParms
        {
            public int QuestionnaireID { get; set; }

            /// <summary>
            /// 当前触发的用户id
            /// </summary>
            public string EventUserID { get; set; }

            public dynamic HttpContextCurrent { get; set; }

            public string CurrfilePath { get; set; }

            public UserInfo SpreadUser { get; set; }

            public string WebsiteOwner { get; set; }

            public void StuViewCount()
            {
                BLLJIMP.BLLMQ bllMq = new BLLJIMP.BLLMQ();

                var msgBody = new BLLJIMP.Model.MQ.QuestionnaireStatisticsInfo()
                {
                    CurrfilePath = this.CurrfilePath,
                    EventBrowser = HttpContextCurrent.Request.Browser == null ? "" : HttpContextCurrent.Request.Browser.ToString(),
                    EventBrowserID = HttpContextCurrent.Request.Browser.Id,
                    EventBrowserIsBata = HttpContextCurrent.Request.Browser.Beta? "测试版": "正式版",
                    EventBrowserVersion = HttpContextCurrent.Request.Browser.Version,
                    EventSysPlatform = HttpContextCurrent.Request.Browser.Platform,
                    EventUserID = this.EventUserID,
                    SourceIP = Common.MySpider.GetClientIP(HttpContextCurrent),
                    QuestionnaireID = this.QuestionnaireID,
                    RequesSourcetUrl = HttpContextCurrent.Request.UrlReferrer != null ? HttpContextCurrent.Request.UrlReferrer.ToString() : "",
                    SourceUrl = HttpContextCurrent.Request.Url.ToString(),
                    SpreadUserId = SpreadUser == null? "": SpreadUser.UserID
                };

                msgBody.IPLocation = Common.MySpider.GetIPLocation(msgBody.SourceIP, HttpContextCurrent);

                if (HttpContextCurrent.Request.Browser.Win16)
                    msgBody.EventSysByte = "16位系统";
                else
                    if (HttpContextCurrent.Request.Browser.Win32)
                    msgBody.EventSysByte = "32位系统";
                else
                    msgBody.EventSysByte = "64位系统";


                var mq = new BLLJIMP.Model.MQ.MessageInfo()
                {
                    Msg = JsonConvert.SerializeObject(msgBody),
                    MsgId = Guid.NewGuid().ToString(),
                    MsgType = CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.MQType.QuestionnaireStatistics),
                    WebsiteOwner = this.WebsiteOwner
                };

                bllMq.Publish(mq);

                //BLLJIMP.BLL bll = new BLLJIMP.BLL();

                //var QuestionnaireModel = bll.Get<BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}", this.QuestionnaireID));

                //MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                //detailInfo.MonitorPlanID = this.QuestionnaireID;
                //detailInfo.EventType = 0;
                //detailInfo.EventBrowser = HttpContextCurrent.Request.Browser == null ? "" : HttpContextCurrent.Request.Browser.ToString();
                //detailInfo.EventBrowserID = HttpContextCurrent.Request.Browser.Id; ;
                //if (HttpContextCurrent.Request.Browser.Beta)
                //    detailInfo.EventBrowserIsBata = "测试版";
                //else
                //    detailInfo.EventBrowserIsBata = "正式版";

                //detailInfo.EventBrowserVersion = HttpContextCurrent.Request.Browser.Version;
                //detailInfo.EventDate = DateTime.Now;
                //if (HttpContextCurrent.Request.Browser.Win16)
                //    detailInfo.EventSysByte = "16位系统";
                //else
                //    if (HttpContextCurrent.Request.Browser.Win32)
                //    detailInfo.EventSysByte = "32位系统";
                //else
                //    detailInfo.EventSysByte = "64位系统";
                //detailInfo.EventSysPlatform = HttpContextCurrent.Request.Browser.Platform;
                //detailInfo.SourceIP = Common.MySpider.GetClientIP(HttpContextCurrent);
                //detailInfo.IPLocation = Common.MySpider.GetIPLocation(detailInfo.SourceIP, HttpContextCurrent);
                //detailInfo.SourceUrl = HttpContextCurrent.Request.Url.ToString();
                //detailInfo.RequesSourcetUrl = HttpContextCurrent.Request.UrlReferrer != null ? HttpContextCurrent.Request.UrlReferrer.ToString() : "";
                //detailInfo.WebsiteOwner = this.WebsiteOwner;
                //detailInfo.ModuleType = "question";
                //detailInfo.EventUserID = this.EventUserID;
                //detailInfo.ShareTimestamp = "1";

                //int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='1' ", this.WebsiteOwner, this.QuestionnaireID));
                //int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='1' ", this.WebsiteOwner, this.QuestionnaireID));
                //int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", this.WebsiteOwner, this.QuestionnaireID));

                //QuestionnaireModel.IP = ipCount;
                //QuestionnaireModel.PV = pvCount;
                //QuestionnaireModel.UV = uvCount;
                //bll.Update(QuestionnaireModel);

                //var spreadUser = this.SpreadUser;

                //if (spreadUser != null)
                //{
                //    detailInfo.SpreadUserID = spreadUser.UserID;

                //    MonitorLinkInfo linkInfo = bll.Get<MonitorLinkInfo>(string.Format(" LinkName='{0}' And MonitorPlanID={1}", spreadUser.UserID, QuestionnaireModel.QuestionnaireID));
                //    if (linkInfo != null)
                //    {
                //        linkInfo.ActivityName = QuestionnaireModel.QuestionnaireName;
                //        linkInfo.ThumbnailsPath = QuestionnaireModel.QuestionnaireImage;
                //        //已经为该用户建立推广链接
                //        detailInfo.LinkID = linkInfo.LinkID;
                //        //增加打开人数
                //        linkInfo.OpenCount++;
                //        int shareCount = bll.GetCount<MonitorEventDetailsInfo>("ShareTimestamp", string.Format(" LinkID ={0} and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ", linkInfo.LinkID));
                //        linkInfo.ShareCount = shareCount;
                //        int iCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID='{0}' AND SpreadUserID='{1}' ", this.QuestionnaireID, spreadUser.UserID));
                //        linkInfo.DistinctOpenCount = ipCount;
                //        int uCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserId ", string.Format(" MonitorPlanID='{0}' AND SpreadUserID='{1}' ", this.QuestionnaireID, spreadUser.UserID));

                //        int spreadCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID='{0}' AND EventUserId='{1}' ", this.QuestionnaireID, this.EventUserID));
                //        if (spreadCount == 0)
                //        {
                //            uCount = uCount + 1;
                //        }

                //        bll.Update(linkInfo, string.Format(" OpenCount={0},DistinctOpenCount={1},UV={2},ShareCount={3}", linkInfo.OpenCount, iCount, uCount, shareCount), string.Format("LinkID={0}", linkInfo.LinkID));
                //    }
                //    else
                //    {

                //        //还没有为该用户建立推广链接
                //        MonitorLinkInfo newLinkinfo = new MonitorLinkInfo();
                //        newLinkinfo.LinkID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorLinkID));
                //        newLinkinfo.MonitorPlanID = QuestionnaireModel.QuestionnaireID;
                //        newLinkinfo.WXMemberID = 0;
                //        newLinkinfo.LinkName = spreadUser.UserID;
                //        newLinkinfo.RealLink = this.CurrfilePath;
                //        newLinkinfo.InsertDate = DateTime.Now;
                //        newLinkinfo.OpenCount = 1;
                //        newLinkinfo.ActivityName = QuestionnaireModel.QuestionnaireName;
                //        newLinkinfo.ThumbnailsPath = QuestionnaireModel.QuestionnaireImage;
                //        newLinkinfo.WebsiteOwner = this.WebsiteOwner;
                //        newLinkinfo.DistinctOpenCount = 1;// ip
                //        newLinkinfo.ShareCount = 0;//分享数
                //        newLinkinfo.ForwardType = "questionnaire";
                //        newLinkinfo.UV = 1;
                //        newLinkinfo.ActivityId = QuestionnaireModel.QuestionnaireID;
                //        bll.Add(newLinkinfo);
                //    }

                //}
                //bll.Add(detailInfo);


                ////更新ip pv uv[Questionnaire]

                //int countIp = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID = {0} ", QuestionnaireModel.QuestionnaireID));
                //int countPv = QuestionnaireModel.PV++;
                //int countUv = bll.GetCount<MonitorEventDetailsInfo>(" EventUserId ", string.Format(" MonitorPlanID = {0} ", QuestionnaireModel.QuestionnaireID));
                //bll.Update(QuestionnaireModel, string.Format(" IP={0},PV={1},UV={2} ", countIp, countPv, countUv), string.Format(" QuestionnaireID={0} ", QuestionnaireModel.QuestionnaireID));

                ////更新 转发表
                //bll.Update(new ActivityForwardInfo(), string.Format(" PV+=1,UV={0}", countUv), string.Format(" ActivityId='{0}'", QuestionnaireModel.QuestionnaireID));//更新转发表UV.pv



            }

        }
    }
}