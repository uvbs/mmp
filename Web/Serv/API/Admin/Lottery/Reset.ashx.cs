using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery
{
    /// <summary>
    /// Reset 的摘要说明   重置
    /// </summary>
    public class Reset : BaseHandlerNeedLoginAdminNoAction
    {
        /// 刮奖活动BLL
        /// </summary>
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (!string.IsNullOrEmpty(ids))
            {
                resp.isSuccess = true;
                resp.errmsg = "ids 为必填项,请检查";
                bllLottery.ContextResponse(context,resp);
                return;
            }
            WXLotteryV1 model = new WXLotteryV1();
            foreach (var item in ids.Split(','))
            {
                model = bllLottery.Get<WXLotteryV1>(string.Format("LotteryID={0}", item));
                if (model == null || (!model.WebsiteOwner.Equals(bllLottery.WebsiteOwner)))
                {
                    resp.errmsg = "无权访问";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IntegralProblem;
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
            }
            bllLottery.Delete(new WXLotteryLogV1(), string.Format("LotteryId in ({0})", ids));
            bllLottery.Delete(new WXLotteryRecordV1(), string.Format("LotteryId in ({0})", ids));
            ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format(" Update ZCJ_WXAwardsV1 Set WinCount=0 Where LotteryId  in({0})", ids));
            resp.errmsg = "已成功重置";
            resp.isSuccess = true;
            bllLottery.ContextResponse(context, resp);
        }
    }
}