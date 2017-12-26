using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery.wap
{
    public partial class ScratchV1 : System.Web.UI.Page
    {
        /// <summary>
        /// model
        /// </summary>
        public WXLotteryV1 model;
        /// <summary>
        /// 刮奖BLL
        /// </summary>
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        /// <summary>
        /// 是否领奖
        /// </summary>
        public string myCashed = "false";
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string myAwardName = "";
        /// <summary>
        /// 是否中奖
        /// </summary>
        public string myIsAward = "undefined";
        /// <summary>
        /// 是否显示变量
        /// </summary>
        //public bool IsShowVar;
        /// <summary>
        /// 是否结束
        /// </summary>
        public string myAwardGameOver = "false";
        /// <summary>
        /// 开始时间
        /// </summary>
        public string myStartTime;
        /// <summary>
        /// 领奖方式 1现场领奖2后台设置领奖3提交信息领奖
        /// </summary>
        public string myPageCash = "1";
        /// <summary>
        /// 当前用户
        /// </summary>
        UserInfo CurrentUserInfo;
        /// <summary>
        /// 是否已经提交过领奖信息（姓名，手机）
        /// </summary>
        public string isSubmitInfo = "false";
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!bll.IsLogin) || (!bll.IsMobile))
            {
                Response.Write("请用微信打开");
                Response.End();

            }
            try
            {
                CurrentUserInfo = bll.GetCurrentUserInfo();
                //if (string.IsNullOrEmpty(CurrentUserInfo.WXNickname))
                //{
                //    Session.Clear();
                //    Response.Redirect(Request.Url.ToString());
                //    Response.End();
                //    return;
                //}
                
                model = bll.Get<WXLotteryV1>(string.Format("LotteryID={0}", Request["id"]));
                if (model == null)
                {
                    Response.End();
                }


                var signInMsg = string.Empty;
                var signInCanUse = new BllLottery().signInLotteryIsCanUse(model.LotteryID, out signInMsg);

                if (!signInCanUse)
                {
                    Response.Redirect("/Error/CommonMsg.aspx?msg="+ signInMsg);
                    Response.End();
                }


                myStartTime = ((DateTime)model.StartTime).ToString("yyyy/MM/dd HH:mm:ss");
                if (model.IsGetPrizeFromMobile.Equals(0))
                {
                    myPageCash = "1";
                }
                if (model.IsGetPrizeFromMobile.Equals(1))
                {
                    myPageCash = "2";
                }
                if (model.IsGetPrizeFromMobile.Equals(2))
                {
                    myPageCash = "3";
                }
                //myPageCash=model.IsGetPrizeFromMobile.Equals(1)?"true":"false";
                if (model.Status == 1)
                {

                }
                else
                {
                    myAwardGameOver = "true";

                }
                var lotteryRecord = bll.GetWXLotteryRecordV1(CurrentUserInfo.UserID, model.LotteryID);
                if (lotteryRecord != null)
                {
                    //IsShowVar = true;
                    myAwardName = lotteryRecord.WXAwardName;
                    myCashed = IsUserGetPrizeV1(CurrentUserInfo.UserID, model.LotteryID).ToString().ToLower();
                    myIsAward = "true";
                    if ((!string.IsNullOrEmpty(lotteryRecord.Name)) && (!string.IsNullOrEmpty(lotteryRecord.Phone)))
                    {
                        isSubmitInfo = "true";
                    }


                }
                else
                {
                    int count = bll.GetWXLotteryLogCountV1(model.LotteryID, CurrentUserInfo.UserID);
                    if (count >= model.MaxCount)
                    {
                        //IsShowVar = true;
                        myAwardName = "未中奖";
                        myCashed = "false";
                        myIsAward = "false";
                    }
                }
                MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                detailInfo.MonitorPlanID = model.LotteryID;
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
                detailInfo.RequesSourcetUrl = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() : "";
                detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
                detailInfo.WebsiteOwner = bll.WebsiteOwner;
                detailInfo.ModuleType = "scratch";
                if (bll.IsLogin)
                {
                    detailInfo.EventUserID = bll.GetCurrUserID();
                }
                bll.Add(detailInfo);

                int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, model.LotteryID));
                int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, model.LotteryID));
                int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, model.LotteryID));

                bll.Update(new WXLotteryV1(), string.Format(" IP={0},PV={1},UV={2} ", ipCount, pvCount, uvCount), string.Format(" LotteryID={0} ", model.LotteryID));



            }
            catch (Exception ex)
            {


                Response.Write(ex.ToString());
                Response.End();
            }
        }

        /// <summary>
        /// 检查是否领过奖品
        /// </summary>
        /// <param name="UserId">当前用户的id</param>
        /// <param name="autoId">活动编号</param>
        /// <returns>返回是否领奖成功</returns>
        private bool IsUserGetPrizeV1(string UserId, int autoId)
        {
            WXLotteryRecordV1 wxlRecord = bll.Get<WXLotteryRecordV1>(" UserId='" + UserId + "' And LotteryId=" + autoId);
            if (wxlRecord != null)
            {
                if (wxlRecord.IsGetPrize == 1)
                {
                    return true;
                }
            }
            return false;
        }

    }
}