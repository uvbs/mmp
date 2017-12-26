using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// SetNewPayPwd 的摘要说明
    /// </summary>
    public class SetNewPayPwd : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string pay_pwd = context.Request["pay_pwd"];
            string new_pwd = context.Request["new_pwd"];
            string confirm_pwd = context.Request["confirm_pwd"];
            if (string.IsNullOrWhiteSpace(pay_pwd))
            {
                apiResp.msg = "请输入原支付密码";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.PayPassword != pay_pwd)
            {
                apiResp.msg = "原支付密码错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(new_pwd))
            {
                apiResp.msg = "请输入新支付密码";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(confirm_pwd))
            {
                apiResp.msg = "请输入新支付密码";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (new_pwd != confirm_pwd)
            {
                apiResp.msg = "两次输入的密码不一致";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Update(CurrentUserInfo, string.Format(" PayPassword='{0}'", pay_pwd),
                string.Format(" WebsiteOwner='{0}'AND AutoID='{1}'", bllUser.WebsiteOwner, CurrentUserInfo.AutoID)) > 0)
            {
                apiResp.msg = "支付密码修改成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "支付密码修改失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}