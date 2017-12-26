using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.Common;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Member
{
    /// <summary>
    /// GetSMSCode 的摘要说明
    /// </summary>
    public class GetSMSCode : BaseHandlerNoAction
    {
        /// <summary>
        /// 短信业务逻辑Bll
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string phone = context.Request["phone"];
            string smsContent = context.Request["smscontent"];
            string check_user = context.Request["check_user"];
            string limit_user = context.Request["limit_user"];
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
            if (check_user == "1")
            {
                UserInfo model = bllUser.GetUserInfoByPhone(phone);
                if (limit_user == "1" && model == null)
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "该手机号没有账号";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
                if (limit_user == "2" && model != null)
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "该手机号已有账号";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
                if (model != null)
                {
                    UserInfo CurrentUserInfo = bllUser.GetCurrentUserInfo();
                    if (CurrentUserInfo !=null && model.UserID != CurrentUserInfo.UserID)
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
            }
            #endregion
            var lastSmsVerificationCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerificationCode != null)
            {
                if ((DateTime.Now - lastSmsVerificationCode.InsertDate).TotalSeconds < 60)
                {
                    apiResp.code = (int)APIErrCode.IsRepeat;
                    apiResp.msg = "验证码限制每60秒发送一次";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
            }
            string verCode = new Random().Next(111111, 999999).ToString();
            string smsSignature = string.Format("{0}", bllSms.GetWebsiteInfoModelFromDataBase().SmsSignature);//短信签名
            if (string.IsNullOrWhiteSpace(smsContent) || !smsContent.Contains("{{SMSVERCODE}}")) smsContent = "手机验证码：{{SMSVERCODE}}";
            smsContent = smsContent.Replace("{{SMSVERCODE}}", verCode);//替换验证码标签
            string msg = "";
            bool isSuccess = false;
            bllSms.SendSmsVerificationCode(phone, smsContent, smsSignature, verCode, out isSuccess, out msg);
            if (!isSuccess)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "手机验证码发送失败";
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = isSuccess;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "手机验证码已发送";
            bllSms.ContextResponse(context, apiResp);
        }
    }
}