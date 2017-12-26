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
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLUserExpand bllUserEx = new BLLUserExpand();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];

            UserExpandType nType = new UserExpandType();
            if (!Enum.TryParse(type, out nType))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "类型格式不能识别";
                bllUserEx.ContextResponse(context, apiResp);
                return;
            }
            UserExpand ex = bllUserEx.GetUserExpand(nType, CurrentUserInfo.UserID);
            if (ex == null) ex = new UserExpand();
            apiResp.result = new
            {
                value = ex.DataValue,
                ex1 = ex.Ex1,
                ex2 = ex.Ex2,
                ex3 = ex.Ex3,
                ex4 = ex.Ex4,
                ex5 = ex.Ex5,
                ex6 = ex.Ex6,
                ex7 = ex.Ex7,
                ex8 = ex.Ex8,
                ex9 = ex.Ex9,
                ex10 = ex.Ex10
            };
            apiResp.msg = "获取完成";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
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