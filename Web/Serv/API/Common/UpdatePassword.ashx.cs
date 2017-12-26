using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// UpdatePassword 的摘要说明
    /// </summary>
    public class UpdatePassword : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string phone = context.Request["phone"];
            string code = context.Request["code"];
            string password = context.Request["pwd"];
            string websiteOwner = bllUser.WebsiteOwner;
            if (string.IsNullOrWhiteSpace(password))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "请输入新密码";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo ouser = bllUser.GetUserInfoByPhone(phone, websiteOwner);
            if (ouser == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "手机号未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            var lastSmsVerCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerCode == null)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "请先获取手机验证码";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (lastSmsVerCode.VerificationCode != code)
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
            ouser.Password = password;
            if (bllUser.Update(ouser, string.Format("Password='{0}'", ouser.Password),
                string.Format("AutoID={0}", ouser.AutoID)) <= 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "手机密码设置失败";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "手机密码设置完成";
            bllUser.ContextResponse(context, apiResp);
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