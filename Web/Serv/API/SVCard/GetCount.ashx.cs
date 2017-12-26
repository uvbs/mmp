using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.SVCard
{
    /// <summary>
    /// GetCount 的摘要说明
    /// </summary>
    public class GetCount : BaseHandlerNeedLoginNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string status = context.Request["status"]; //0未使用 1已使用 2已转赠 3已过期
            string websiteOwner = bll.WebsiteOwner;
            string curUserId = CurrentUserInfo.UserID;
            int total = bll.GetRecordCount(null, websiteOwner, status, curUserId);
            if (status=="0")
            {
               total= bll.GetCanUseStoredValueCardList(CurrentUserInfo.UserID).Count;
            }
            apiResp.msg = "前台查询储值卡数量";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = total;
            bll.ContextResponse(context, apiResp);
        }
    }
}