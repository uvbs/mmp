using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// ResetWinning 的摘要说明  抽奖重置
    /// </summary>
    public class ResetWinning : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string lotteryId = context.Request["lottery_id"];
            if (string.IsNullOrEmpty(lotteryId))
            {
                apiResp.msg = "中奖id为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            WXLotteryV1 model = bllUser.Get<WXLotteryV1>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}", bllUser.WebsiteOwner, lotteryId));
            if (model == null)
            {
                apiResp.msg = "抽奖活动不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            bool result = bllUser.Update(new BLLJIMP.Model.LotteryUserInfo(), string.Format(" IsWinning=0 "), string.Format(" WebsiteOwner='{0}' AND LotteryId={1} ", bllUser.WebsiteOwner, lotteryId)) > 0;
            
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