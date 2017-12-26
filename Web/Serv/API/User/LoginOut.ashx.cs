using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 退出登录
    /// </summary>
    public class LoginOut : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 用户处理逻辑BLL
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            context.Session.Clear();
            resp.errcode = 0;
            resp.errmsg = "ok";
        //    if (!bllUser.IsLogin)
        //    {
        //        resp.errcode = 1;
        //        resp.errmsg = "尚未登录";
        //        goto outoff;
        //    }
        //    else
        //    {
        //        resp.errcode = 0;
        //        resp.errmsg = "ok";
        //    }

        //outoff:
            result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }
        }

        /// <summary>
        /// 默认响应模型
        /// </summary>
        public class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

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