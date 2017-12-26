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
    /// 删除默认中奖名单
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (bllLottery.DeleteMultByKey<WXLotteryWinningDataV1>("AutoID", ids) > 0)
            {
                resp.isSuccess = true;
                resp.errcode = (int)APIErrCode.IsSuccess;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "删除默认中奖出错";
            }
            bllLottery.ContextResponse(context, resp);
        }
    }
}