using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    ///后台管理基处理 需要登录 
    /// </summary>
    public class BaseHandlerNeedLoginAdminNoAction : IHttpHandler, IReadOnlySessionState
    {

        /// <summary>
        /// 用户BLL
        /// </summary>
        protected BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected UserInfo currentUserInfo;
        /// <summary>
        /// 默认响应模型 旧
        /// </summary>
        protected DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;

            try
            {


            if (!bllUser.IsLogin)
            {
                apiResp.code = (int)APIErrCode.UserIsNotLogin;
                apiResp.msg = "请先登录";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            else
            {

                currentUserInfo = bllUser.GetCurrentUserInfo();
                if (
                    currentUserInfo.UserID == bllUser.WebsiteOwner
                    ||
                    currentUserInfo.UserType == 1
                    ||
                    currentUserInfo.IsSubAccount == "1"
                    ||
                    currentUserInfo.UserType == 7
                    )
                {

                }
                else
                {
                    apiResp.code = -4;
                    apiResp.msg = "无权访问";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }

            }
            this.GetType().GetMethod("ProcessRequest").Invoke(this, new[] { context });
            }
            catch (Exception ex)
            {
                apiResp.code = -1;
                //if (ex.InnerException != null)
                //{
                //    apiResp.msg = ex.InnerException.Message;
                //}
                //else
                //{
                //    apiResp.msg = ex.Message;
                //}
                apiResp.msg = ex.ToString();
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
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
