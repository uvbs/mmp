using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// EmailBackPwd 的摘要说明
    /// </summary>
    public class EmailBackPwd : BaseHandlerNoAction
    {
        BLLJIMP.BLL bllUser = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string email = context.Request["email"];
            string code = context.Request["code"];
            string newPwd = context.Request["new_pwd"];
            if(string.IsNullOrEmpty(email))
            {
                resp.errmsg = "email 为必填项,请检查";
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
            //检查邮箱格式是否正确

            //检查密码格式是否有8位

            //检查验证码是否正确

            //如果以上全部通过就修改密码
            

        }
    }
}