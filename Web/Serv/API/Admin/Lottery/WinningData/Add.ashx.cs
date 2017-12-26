using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.WinningData
{
    /// <summary>
    /// 添加默认中奖名单
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {

            string lotteryId = context.Request["lottery_id"];
            string userId = context.Request["user_id"];
            string awardId = context.Request["award_id"];
            if (string.IsNullOrEmpty(lotteryId))
            {
                resp.errcode = (int)APIErrCode.IsNotFound;
                resp.errmsg = "活动id为必填项，请检查";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(userId))
            {
                resp.errcode = (int)APIErrCode.IsNotFound;
                resp.errmsg = "用户id为必填项，请检查";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(awardId))
            {
                resp.errcode = (int)APIErrCode.IsNotFound;
                resp.errmsg = "奖项id为必填项，请检查";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (bllUser.GetUserInfo(userId) == null)
            {
                resp.errcode = (int)APIErrCode.IsNotFound;
                resp.errmsg = "用户名不存在，请检查";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            if (bllLottery.ExistsWinningData(lotteryId, userId))
            {
                resp.errcode = (int)APIErrCode.IsRepeat;
                resp.errmsg = "该用户名已经在中奖名单中";
                bllLottery.ContextResponse(context, resp);
                return;
            }
            int AwardCount = bllLottery.GetByKey<WXAwardsV1>("AutoID", awardId).PrizeCount;
            if (AwardCount - bllLottery.GetCountByKey<WXLotteryWinningDataV1>("WXAwardsId", awardId) - 1 < 0)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "默认中奖奖项之和超过了该奖项的数量";
                bllLottery.ContextResponse(context, resp);
                return;
            }

            WXLotteryWinningDataV1 model = new WXLotteryWinningDataV1();
            model.LotteryId = Convert.ToInt32(lotteryId);
            model.UserId = userId;
            model.WXAwardsId = Convert.ToInt32(awardId);
            if (bllLottery.Add(model))
            {
                resp.isSuccess = true;
                resp.errcode = (int)APIErrCode.IsSuccess;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "添加失败";
            }
            bllLottery.ContextResponse(context, resp);
        }

    }
}