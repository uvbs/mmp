using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// SetLotteryInfo 的摘要说明
    /// </summary>
    public class SetLotteryInfo : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string lotteryId = context.Request["lottery_id"];
            if (string.IsNullOrEmpty(lotteryId))
            {
                apiResp.msg = "抽奖活动id为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            bool result = bllUser.Delete(new BLLJIMP.Model.LotteryUserInfo(), string.Format(" WebsiteOwner='{0}' AND LotteryId={1}", bllUser.WebsiteOwner, lotteryId)) > 0;

            if (result)
            {
                apiResp.msg = "重置成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "重置失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}