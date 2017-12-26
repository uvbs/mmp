using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// CheckPayPwd 的摘要说明
    /// </summary>
    public class CheckPayPwd : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string pay_pwd = context.Request["pay_pwd"];
            if (string.IsNullOrWhiteSpace(pay_pwd))
            {
                apiResp.msg = "请输入支付密码";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.PayPassword != pay_pwd)
            {
                apiResp.msg = "支付密码错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.msg = "支付密码验证通过";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            bllUser.ContextResponse(context, apiResp);
        }

    }
}