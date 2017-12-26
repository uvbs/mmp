using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// ForgetPayPassword 的摘要说明
    /// </summary>
    public class ForgetPayPassword : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string vcode = context.Request["vcode"];
            string pay_pwd = context.Request["pay_pwd"];
            string websiteOwner = bllUser.WebsiteOwner;

            if (string.IsNullOrWhiteSpace(pay_pwd))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请设置支付密码";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            var lastSmsVerCode = bllSms.GetLastSmsVerificationCode(CurrentUserInfo.Phone);
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
                apiResp.msg = "手机短信验证码错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if ((DateTime.Now - lastSmsVerCode.InsertDate).TotalMinutes >= 5)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "手机短信验证码已过期,请重新获取";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Update(CurrentUserInfo, string.Format("PayPassword='{0}'", pay_pwd),
                string.Format("AutoID={0}", CurrentUserInfo.AutoID)) <= 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "修改支付密码失败";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "修改支付密码完成";
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