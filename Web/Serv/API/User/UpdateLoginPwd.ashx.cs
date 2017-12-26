using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// UpdateLoginPwd 的摘要说明  修改登录密码
    /// </summary>
    public class UpdateLoginPwd : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string userPwd = context.Request["user_pwd"];
            string newPwd = context.Request["new_pwd"];
            string configPwd = context.Request["config_pwd"];
            string pwdLength = context.Request["pwd_length"];
            string verCode = context.Request["ver_code"];
            #region 验证
            if (string.IsNullOrEmpty(userPwd))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "user_pwd 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (!string.IsNullOrEmpty(verCode))
            {
                if (verCode != context.Session["CheckCode"].ToString().ToLower())
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "VerCode Error";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

            }
            if (string.IsNullOrEmpty(userPwd))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "new_pwd 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(configPwd))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "config_pwd 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (newPwd != configPwd)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "两次输入的密码不一致";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (!string.IsNullOrEmpty(pwdLength))
            {
                if (newPwd.Length < int.Parse(pwdLength))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "密码需要" + pwdLength + "位以上长度";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            if (CurrentUserInfo.Password != userPwd)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "原始密码错误";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion
            CurrentUserInfo.Password = newPwd;
            if (bllUser.UpdatePassword(CurrentUserInfo))
            {
                resp.isSuccess = true;
                resp.errmsg = "修改密码完成";
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