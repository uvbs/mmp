using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// ResetPassword 的摘要说明
    /// </summary>
    public class ResetPassword : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string phone = context.Request["phone"];
            string vcode = context.Request["vcode"];
            string websiteOwner = bllUser.WebsiteOwner;

            var lastSmsVerCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerCode == null)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "请先获取手机验证码";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (lastSmsVerCode.VerificationCode != vcode)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "手机验证码错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if ((DateTime.Now - lastSmsVerCode.InsertDate).TotalMinutes >= 5)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "手机验证码已过期,请重新获取";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            BLLJIMP.Model.UserInfo ouser = bllUser.GetUserInfoByPhone(phone, websiteOwner);
            ouser.Password = ZentCloud.Common.Rand.Number(6);
            bool smsBool = false;
            string smsMsg = "";
            BLLJIMP.Model.WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            bllSms.SendSmsMisson(ouser.Phone, "您的密码是：" + ouser.Password, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), website.SmsSignature, out smsBool, out smsMsg);
            if (!smsBool)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "发送短信密码失败";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Update(ouser, string.Format("Password='{0}'", ouser.Password), 
                string.Format("AutoID={0}", ouser.AutoID))<=0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "重置失败";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "重置完成，新密码将发送到您的手机";
            bllUser.ContextResponse(context, apiResp);
        }
    }
}