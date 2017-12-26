using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// 检查是否登录
    /// </summary>
    public class IsLogin : BaseHandlerNoAction
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string result = "";
            if (bllUser.IsLogin)
            {
                    result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                    {
                        is_login = true,
                        user_name = bllUser.GetCurrUserID()
                    });
                
            }
            else
            {
                result = ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    is_login = false,
                    user_name = ""
                });
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                result = string.Format("{0}({1})", context.Request["callback"], result);
            }
            else
            {
                //返回json数据
            }
            context.Response.Write(result);
        }

    }
}