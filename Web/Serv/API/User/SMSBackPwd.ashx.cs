using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// SMSBackPwd 的摘要说明   手机找回登录密码
    /// </summary>
    public class SMSBackPwd : BaseHandlerNoAction
    {
        /// <summary>
        /// 短信业务逻辑Bll
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");

        BLLJIMP.BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string phone = context.Request["phone"];
            string code = context.Request["code"];
            string newPwd = context.Request["new_pwd"];
            string configPwd = context.Request["confirm_pwd"];
            string auto_login = context.Request["auto_login"];
            string pwdLength=context.Request["pwd_length"];
            if (string.IsNullOrEmpty(phone))
            {
                resp.errmsg = "phone 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(code))
            {
                resp.errmsg = "code 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(newPwd))
            {
                resp.errmsg = "newPwd 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(configPwd))
            {
                resp.errmsg = "confirm 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #region 手机格式验证
            if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                resp.errmsg = "手机号码出错";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion

            #region 密码检查
            //输入是否一致
            if (configPwd != newPwd)
            {
                resp.errmsg = "两次的密码输入不一致";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (!string.IsNullOrEmpty(pwdLength))
            {
                if (newPwd.Length < int.Parse(pwdLength))
                {
                    resp.errmsg = "长度不够";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            else
            {
                //密码长度
                if (newPwd.Length < 8)
                {
                    resp.errmsg = "长度不够";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            

            #endregion

            UserInfo model = bllUser.GetUserInfoByPhone(phone);
            if (model == null)
            {
                resp.errmsg = "账号未找到";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            #region 判断验证码是否正确
            SmsVerificationCode sms=bllSms.GetLastSmsVerificationCode(phone);
            if (sms.VerificationCode!=code)
            {
                resp.errmsg = "验证码错误";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion

            if (bllUser.Update(model, string.Format(" Password='{0}'", newPwd), string.Format(" Phone='{0}'", phone)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "修改密码成功";

                if (auto_login == "1")
                {
                    context.Session[SessionKey.UserID] = model.UserID;
                    context.Session[SessionKey.LoginStatu] = 1;
                    context.Response.Cookies.Add(bllUser.CreateLoginCookie(model.UserID, model.WXOpenId, model.WXNickname));
                }
            }
            else
            {
                resp.errmsg = "修改密码出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}