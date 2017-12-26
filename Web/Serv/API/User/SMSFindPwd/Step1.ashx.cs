using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.SMSFindPwd
{
    /// <summary>
    /// Step1 的摘要说明
    /// </summary>
    public class Step1 : BaseHandlerNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLSMS bllSms = new BLLJIMP.BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string phone = context.Request["phone"];//手机号
            string verCode = context.Request["ver_code"];//验证码

            if (string.IsNullOrEmpty(phone))
            {
                apiResp.msg = "Please Type The Cell Phone";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            if (string.IsNullOrEmpty(verCode))
            {
                apiResp.msg = "Please Type The Code";
                bllUser.ContextResponse(context, apiResp);
                return;

            }
            if (context.Session["CheckCode"]==null)
            {
                apiResp.msg = "CheckCode NULL";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (verCode != context.Session["CheckCode"].ToString().ToLower())
            {
                apiResp.msg = "Code Error";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            //发送验证码到手机
            bool isSuccess = false;
            string smsVerCode = new Random().Next(111111, 999999).ToString();
            string msg = "";

            string smsContent = string.Format("Your verification code {0}", smsVerCode);
            
            string smsSignature = string.Format("{0}", bllSms.GetWebsiteInfoModelFromDataBase().SmsSignature);//短信签名

            bllSms.SendSmsVerificationCode(phone, smsContent, smsSignature, smsVerCode, out isSuccess, out msg);
            if (isSuccess)
            {
                apiResp.status = true;
                context.Session["Phone"]=phone;
            }
            else
            {
                
                apiResp.msg = "Send Fail";
            }
            //发送验证码到手机
            bllUser.ContextResponse(context, apiResp);


        }


    }
}