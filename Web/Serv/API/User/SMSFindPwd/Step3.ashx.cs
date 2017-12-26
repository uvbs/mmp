using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.SMSFindPwd
{
    /// <summary>
    /// Step3 的摘要说明
    /// </summary>
    public class Step3 : BaseHandlerNoAction
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLSMS bllSms = new BLLJIMP.BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string password = context.Request["password"];//手机验证码
            string passwordConfirm = context.Request["passwordconfirm"];
            if (string.IsNullOrEmpty(password))
            {
                apiResp.msg = "Please Type new password";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            if (string.IsNullOrEmpty(passwordConfirm))
            {
                apiResp.msg = "Please Type new password again";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            if (password != passwordConfirm)
            {
                apiResp.msg = "our confirmed password and new password do not match";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (context.Session["Phone"]==null)
            {
                apiResp.msg = "Phone Is Null";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (context.Session["FindPasswordResult"] == null)
            {
                apiResp.msg = "FindPasswordResult Error";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            string phone = context.Session["Phone"].ToString();
            if (context.Session["FindPasswordResult"].ToString() == "true" && !string.IsNullOrEmpty(phone))
            {
                var userInfo = bllUser.GetUserInfoByPhone(phone, bllUser.WebsiteOwner);

                if (bllUser.Update(new BLLJIMP.Model.UserInfo(), string.Format(" Password='{0}'",password), string.Format(" AutoId={0}", userInfo.AutoID)) > 0)
                {


                    context.Session[SessionKey.LoginStatu] = 1;
                    context.Session[SessionKey.UserID] = userInfo.UserID;
                    apiResp.status = true;

                }
                else
                {
                    apiResp.msg = "operation failed";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }

            }
            else
            {
                apiResp.msg = "Check Error";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            bllUser.ContextResponse(context, apiResp);


        }
    }
}