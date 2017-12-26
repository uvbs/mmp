using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// Delete 的摘要说明     删除抽奖活动参与者
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {

             string autoIds=context.Request["autoids"];
             string lotteryId=context.Request["lottery_id"];
             if (string.IsNullOrEmpty(autoIds))
             {
                 apiResp.msg = "参与者id为空";
                 apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                 bllUser.ContextResponse(context, apiResp);
                 return;
             }

             bool result = bllUser.Delete(new BLLJIMP.Model.LotteryUserInfo(), string.Format(" WebsiteOwner='{0}' AND AutoId in ({1})", bllUser.WebsiteOwner, autoIds))>0;

             if (result)
             {
                 int count = bllUser.GetCount<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}", bllUser.WebsiteOwner, lotteryId));

                 bllUser.UpdateByKey<WXLotteryV1>("LotteryID", lotteryId, "WinnerCount", count.ToString());
                 apiResp.msg = "删除参与者成功";
                 apiResp.status = true;
             }
             else
             {
                 apiResp.msg = "删除参与者失败";
                 apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
             }
             bllUser.ContextResponse(context, apiResp);
        }
    }
}