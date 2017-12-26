using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.SMSFindPwd
{
    /// <summary>
    /// Step2 的摘要说明
    /// </summary>
    public class Step2 : BaseHandlerNoAction
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLSMS bllSms = new BLLJIMP.BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request["code"];//手机验证码
            if (string.IsNullOrEmpty(code))
            {
                apiResp.msg = "Please Type The Cell Phone";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            if (string.IsNullOrEmpty(code))
            {
                apiResp.msg = "Please Type The Code";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            if (context.Session["Phone"]==null)
            {
                apiResp.msg = "Phone Is Null";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            var phone = context.Session["Phone"].ToString();
            string lastCode = bllSms.GetLastSmsVerificationCode(phone).VerificationCode;
            if (code != lastCode)
            {
                apiResp.msg = "Code Error";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            else
            {
                context.Session["FindPasswordResult"]="true";
                apiResp.status = true;
                bllUser.ContextResponse(context, apiResp);
            }
            bllUser.ContextResponse(context, apiResp);


        }
    }
}