using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ScoreDefine
{
    /// <summary>
    ///删除积分规则
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            if (bllScoreDefine.DeleteScoreDefine(type, bllScoreDefine.WebsiteOwner))
            {
                resp.isSuccess = true;
                resp.errcode = (int)APIErrCode.IsSuccess;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "删除失败";
            }
            bllScoreDefine.ContextResponse(context, resp);
        }
    }
}