using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// UpdatePayPwd 的摘要说明  修改支付密码
    /// </summary>
    public class UpdatePayPwd : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string ex1 = context.Request["ex1"];
            string newEx1 = context.Request["new_ex1"];
            string configEx1 = context.Request["config_ex1"];
            if (string.IsNullOrEmpty(ex1))
            {
                resp.errmsg = "ex1 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(newEx1))
            {
                resp.errmsg = "new_ex1 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(configEx1))
            {
                resp.errmsg = "config_ex1 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (newEx1 != configEx1)
            {
                resp.errmsg = "两次输入的密码不一致";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (CurrentUserInfo.Ex1 != ex1)
            {
                resp.errmsg = "支付密码错误";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllUser.Update(CurrentUserInfo, string.Format(" Ex1='{0}'", newEx1), string.Format(" WebsiteOwner='{0}'AND AutoID='{1}'",bllUser.WebsiteOwner,CurrentUserInfo.AutoID))>0)
            {
                resp.isSuccess = true;
                resp.errmsg = "修改支付密码成功";
            }
            else
            {
                resp.errmsg = "修改支付密码出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}