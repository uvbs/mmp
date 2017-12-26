using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// Summary description for ReviewHandler
    /// </summary>
    public class BaseHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 统一响应模型
        /// </summary>
        protected AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected ZentCloud.BLLJIMP.Model.UserInfo currUserInfo; //当前登陆的用户
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            this.currUserInfo = DataLoadTool.GetCurrUserModel();
            
            try
            {
                string action = context.Request["Action"];

                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "Action为空！";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
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