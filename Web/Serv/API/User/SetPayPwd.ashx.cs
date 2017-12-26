using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// SetPayPwd 的摘要说明
    /// </summary>
    public class SetPayPwd : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string pay_pwd = context.Request["pay_pwd"];
            if(string.IsNullOrWhiteSpace(pay_pwd)){
                apiResp.msg = "请输入支付密码";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context,apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(CurrentUserInfo.PayPassword))
            {
                apiResp.msg = "账户已设置支付密码";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context,apiResp);
                return;
            }
            CurrentUserInfo.PayPassword = pay_pwd;
            if(bllUser.Update(CurrentUserInfo, string.Format(" PayPassword='{0}'", pay_pwd), 
                string.Format(" WebsiteOwner='{0}'AND AutoID='{1}'",bllUser.WebsiteOwner,CurrentUserInfo.AutoID))>0){
                    apiResp.msg = "支付密码设置成功";
                    apiResp.code = (int)APIErrCode.IsSuccess;
                    apiResp.status = true;
                }
            else
            {
                apiResp.msg = "支付密码设置失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}