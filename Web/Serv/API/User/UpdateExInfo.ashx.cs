using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 更新用户扩展信息
    /// </summary>
    public class UpdateExInfo : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string ex1 = context.Request["ex1"];
            string ex2 = context.Request["ex2"];
            string ex3 = context.Request["ex3"];
            string ex4 = context.Request["ex4"];
            string ex5 = context.Request["ex5"];

            CurrentUserInfo.Ex1 = ex1;
            CurrentUserInfo.Ex2 = ex2;
            CurrentUserInfo.Ex3 = ex3;
            CurrentUserInfo.Ex4 = ex4;
            CurrentUserInfo.Ex5 = ex5;

            if (bllUser.Update(CurrentUserInfo))
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "更新失败";
            }

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        
    }
}