using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Booking
{
    /// <summary>
    /// SetAccessLevel 的摘要说明
    /// </summary>
    public class SetAccessLevel : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string pid = context.Request["pid"];
            string access_level = context.Request["access_level"];
            if (bllMall.UpdateMultByKey<WXMallProductInfo>("PID", pid, "AccessLevel", access_level,null,true) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "修改完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "修改失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllMall.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}