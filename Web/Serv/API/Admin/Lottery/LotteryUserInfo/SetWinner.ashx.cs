using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// SetWinner 的摘要说明   修改中奖状态
    /// </summary>
    public class SetWinner : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string id=context.Request["id"];
            string lotteryId=context.Request["lottery_id"];
            if (string.IsNullOrEmpty(id))
            {
                apiResp.msg = "id不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            BLLJIMP.Model.LotteryUserInfo model = bllUser.Get<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}",bllUser.WebsiteOwner,id));
            if (model == null)
            {
                apiResp.msg = "抽奖用户不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            model.IsWinning = 1;
            model.WinnerDate = DateTime.Now;
            int count = bllUser.GetCount<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryId={1} AND IsWinning=1", bllUser.WebsiteOwner, lotteryId));
            model.Number = count+1;
            if(bllUser.Update(model))
            {
                apiResp.msg = model.Number.ToString();
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "中奖失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail; ;
            }
            bllUser.ContextResponse(context,apiResp);

        }

      
    }
}