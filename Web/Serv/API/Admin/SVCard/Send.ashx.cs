using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard
{
    /// <summary>
    /// 发放储值卡
    /// </summary>
    public class Send : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        BLLWeixin bllWeixin = new BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            string card_id = context.Request["id"];
            string type = context.Request["type"];
            string user_ids = context.Request["user_ids"];
            string tags = context.Request["tags"];
            string websiteOwner = bll.WebsiteOwner;

            string msg = "";
            string authority = context.Request.Url.Authority;
            if (bll.SendRecord(card_id, type, user_ids, tags, websiteOwner, currentUserInfo.UserID, out msg, authority))
            {
                apiResp.status = true;
                apiResp.msg = "发放储值卡完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = msg;
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}