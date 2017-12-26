using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Expand
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginNoAction
    {
        BLLUserExpand bllUserEx = new BLLUserExpand();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            string value = context.Request["value"];
            string ex1 = context.Request["ex1"];
            string ex2 = context.Request["ex2"];
            string ex3 = context.Request["ex3"];
            string ex4 = context.Request["ex4"];
            string ex5 = context.Request["ex5"];
            string ex6 = context.Request["ex6"];
            string ex7 = context.Request["ex7"];
            string ex8 = context.Request["ex8"];
            string ex9 = context.Request["ex9"];
            string ex10 = context.Request["ex10"];
            UserExpandType nType = new UserExpandType();
            if (!Enum.TryParse(type, out nType))
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "类型格式不能识别";
                bllUserEx.ContextResponse(context, apiResp);
                return;
            }
            if (bllUserEx.UpdateUserExpand(nType, CurrentUserInfo.UserID, value, ex1, ex2, ex3, ex4, ex5, ex6, ex7, ex8, ex9, ex10))
            {
                apiResp.msg = "修改完成";
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "修改失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllUserEx.ContextResponse(context, apiResp);
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