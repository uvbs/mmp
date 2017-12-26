using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Member
{
    /// <summary>
    /// PostVerify 的摘要说明
    /// </summary>
    public class PostVerify : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 短信业务逻辑Bll
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWebSite = new BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            string phone = context.Request["phone"];
            string code = context.Request["code"];
            #region 判断手机格式
            if (!MyRegex.PhoneNumLogicJudge(phone))
            {
                apiResp.code = (int)APIErrCode.PhoneFormatError;
                apiResp.msg = "手机格式错误";
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            #region 判断手机是否已被使用，且是否是当前账号
            UserInfo model = bllUser.GetUserInfoByPhone(phone);
            if (model != null)
            {
                if (model.UserID != CurrentUserInfo.UserID)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "手机号码已被其他账号使用，请联系管理员";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
                //if (model.IsPhoneVerify == 1)
                //{
                //    apiResp.code = (int)APIErrCode.OperateFail;
                //    apiResp.msg = "手机号码已验证";
                //    bllSms.ContextResponse(context, apiResp);
                //    return;
                //}
            }
            #endregion
            #region 判断验证码是否正确
            SmsVerificationCode sms = bllSms.GetLastSmsVerificationCode(phone);
            if (sms.VerificationCode != code)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "验证码错误";
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            CurrentUserInfo.Phone = phone;
            CurrentUserInfo.IsPhoneVerify = 1;
            CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            if (nWebsiteConfig.MemberStandard == 1)
            {
                if (CurrentUserInfo.AccessLevel < 1) {
                    CurrentUserInfo.AccessLevel = 1;
                    CurrentUserInfo.MemberStartTime = DateTime.Now;
                }
                //CurrentUserInfo.MemberApplyStatus = 9;
            }
            if (bllUser.Update(CurrentUserInfo))
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "手机验证完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "验证失败";
            }
            bllSms.ContextResponse(context, apiResp);
        }

    }
}