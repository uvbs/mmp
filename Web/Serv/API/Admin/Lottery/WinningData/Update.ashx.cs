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
    /// 更新默认中奖名单
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["id"];
            string userId = context.Request["user_id"];
            string awardId = context.Request["award_id"];
            if (string.IsNullOrEmpty(autoId))
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

            WXLotteryWinningDataV1 model = bllLottery.GetByKey<WXLotteryWinningDataV1>("AutoID", autoId);
            model.UserId = userId;
            model.WXAwardsId = Convert.ToInt32(awardId);
            if (bllLottery.Update(model))
            {
                resp.isSuccess = true;
                resp.errcode = (int)APIErrCode.IsSuccess;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "更新失败";
            }
            bllLottery.ContextResponse(context, resp);
        }

    }
}