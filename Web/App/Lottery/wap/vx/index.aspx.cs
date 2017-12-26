using System;
using System.Collections.Generic;
using System.IO;
using ZentCloud.BLLJIMP.Model;
using System.Linq;

namespace ZentCloud.JubitIMP.Web.App.Lottery.wap.vx
{
    public partial class index : System.Web.UI.Page
    {
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        UserInfo currentUserInfo = new UserInfo();
        public AshxResponse resp = new AshxResponse();

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUserInfo = bll.GetCurrentUserInfo();

            GetLotteryInfo();

        }
        
        private void GetLotteryInfo()
        {
            

            int id = Convert.ToInt32(Request["id"]);

            if (id == 0)
            {
                resp.IsSuccess = false;
                resp.Msg = "请传入正确的抽奖活动ID";
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.NoFollow;

                Response.Write(resp.Msg);
                Response.End();
            }

            if (!bll.IsLogin)
            {
                resp.IsSuccess = false;
                resp.Msg = "未登录";
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;

                Response.Write(resp.Msg);
                Response.End();
            }

            var model = bll.GetLottery(id);

            if (model == null)
            {
                resp.IsSuccess = false;
                resp.Msg = "未找到抽奖活动";
                resp.Status = (int)BLLJIMP.Enums.APIErrCode.NoFollow;

                Response.Write(resp.Msg);
                Response.End();
            }

            /*
            
            返回抽奖活动基本信息
            返回当前用户抽奖记录
            返回当前用户中奖情况

            */

            List<WXLotteryLogV1> log = new List<WXLotteryLogV1>();
            List<WXLotteryRecordV1> winRecord = new List<WXLotteryRecordV1>();
            List<WXAwardsV1> awards = new List<WXAwardsV1>();
            
            log = bll.GetUserLotteryLog(id, currentUserInfo.UserID);
            winRecord = bll.GetWXLotteryRecordList(id, currentUserInfo.UserID);
            awards = bll.GetAwardsListV1(id);
          

            //判断当前用户今天还能摇奖多少次
            var luckRest = 0;
            switch (model.LuckLimitType)
            {
                case 0:
                    luckRest = model.MaxCount - log.Count;
                    break;
                case 1:
                    var today = DateTime.Now;
                    var todayCount = log.Count(p => p.InsertDate.Year == today.Year && p.InsertDate.Month == today.Month && p.InsertDate.Day == today.Day);
                    luckRest = model.MaxCount - todayCount;
                    break;
                default:
                    break;
            }

            //查询当前用户的中奖纪录
            var lotteryRecord = winRecord.SingleOrDefault(p => p.UserId == currentUserInfo.UserID);//bll.GetWXLotteryRecordV1(currentUserInfo.UserID, model.LotteryID);

            string currAwardName = string.Empty;//当前中奖的名称
            int currIsCashed = 0;//当前是否已领奖
            int currIsSubmitInfo = 0;//当前是否已提交领奖信息

            if (lotteryRecord != null)
            {
                currAwardName = lotteryRecord.WXAwardName;
                currIsCashed = bll.IsUserGetPrizeV1(currentUserInfo.UserID, model.LotteryID) ? 1 : 0;
                if ((!string.IsNullOrEmpty(lotteryRecord.Name)) && (!string.IsNullOrEmpty(lotteryRecord.Phone)))
                {
                    currIsSubmitInfo = 1;
                }
            }

            resp.IsSuccess = true;
            resp.Result = new
            {
                //抽奖活动基本信息
                id = model.LotteryID,
                name = model.LotteryName,
                content = model.LotteryContent,
                status = model.Status,
                startTime = Common.DateTimeHelper.DateTimeToStr(model.StartTime),
                endTime = Common.DateTimeHelper.DateTimeToStr(model.EndTime),
                shareImg = model.ShareImg,
                shareDesc = model.ShareDesc,
                log = log == null ? null : log.Select(p => new { time = p.InsertDate.ToString() }),
                winRecord = winRecord == null ? null : winRecord.Select(p => new
                {
                    id = p.WXAwardsId,
                    token = p.Token,
                    name = p.WXAward.PrizeName,
                    time = p.InsertDateStr,
                    type = p.WXAward.AwardsType
                }),
                maxCount = model.MaxCount,
                luckLimitType = model.LuckLimitType,
                winLimitType = model.WinLimitType,
                usePoints = model.UsePoints,
                awards = awards,
                luckRest = luckRest,
                toolbarbutton = model.ToolbarButton,
                websiteownername = bll.GetWebsiteInfoModel().WebsiteName,
                currIsAward = lotteryRecord != null ? 1 : 0,
                currAwardName = currAwardName,
                currIsCashed = currIsCashed,
                currIsSubmitInfo = currIsSubmitInfo
            };
            
        }

    }
}