using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw.wap
{
    public partial class Join : System.Web.UI.Page
    {
        /// <summary>
        /// 抽奖 BLL
        /// </summary>
        BLLJIMP.BllLottery bllLotery = new BLLJIMP.BllLottery();
        /// <summary>
        /// 用户 BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 抽奖活动
        /// </summary>
        protected WXLotteryV1 lottery = new WXLotteryV1();
        /// <summary>
        /// 参与者信息
        /// </summary>
        protected LotteryUserInfo lotteryUser = new LotteryUserInfo();
        protected bool isSuccess = false;
        protected string msg = string.Empty;
        /// <summary>
        /// 站点
        /// </summary>
        protected WebsiteInfo webSite = new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["lotteryId"]))
            {
                int lotteryId = Convert.ToInt32(Request["lotteryId"]);

                lottery = bllLotery.Get<WXLotteryV1>(string.Format(" WebsiteOwner='{0}' AND  LotteryID={1} ", bllLotery.WebsiteOwner, lotteryId));

                webSite = bllLotery.GetWebsiteInfoModel();

                var currentUserInfo = bllUser.GetCurrentUserInfo();


                if (string.IsNullOrEmpty(currentUserInfo.WXNickname))
                {
                    Session.Clear();
                    Response.Redirect(Request.Url.ToString());
                }


                lotteryUser = bllUser.Get<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryId={1} AND UserId='{2}' ", bllUser.WebsiteOwner, lotteryId, currentUserInfo.UserID));
                if (lotteryUser == null)
                {
                    lotteryUser = new BLLJIMP.Model.LotteryUserInfo();
                    lotteryUser.WebsiteOwner = bllUser.WebsiteOwner;
                    lotteryUser.CreateDate = DateTime.Now;
                    lotteryUser.WinnerDate = DateTime.Now;
                    lotteryUser.IsWinning = 0;
                    lotteryUser.LotteryId = Convert.ToInt32(lotteryId);
                    lotteryUser.UserId = currentUserInfo.UserID;
                    lotteryUser.WXHeadimgurl = currentUserInfo.WXHeadimgurl;
                    lotteryUser.WXNickname = bllUser.GetUserDispalyName(currentUserInfo);
                    if (bllUser.Add(lotteryUser))
                    {
                        msg = "加入成功";
                        isSuccess = true;
                        int count = bllUser.GetCount<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}", bllUser.WebsiteOwner, lotteryId));
                        bllUser.UpdateByKey<WXLotteryV1>("LotteryID", Request["lotteryId"], "WinnerCount", count.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        msg = "加入失败";
                    }
                }
                else
                {
                    msg = "您已参加抽奖";
                    isSuccess = true;
                }



            }

        }
    }
}