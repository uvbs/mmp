using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// Account 的摘要说明
    /// </summary>
    public class Account : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            if (CurrentUserInfo != null)
            {
                resp.isSuccess = true;
            }
            resp.returnObj = new 
            {
                account=CurrentUserInfo.Account,//账户余额
                ex6=CurrentUserInfo.Ex6//信用金
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}