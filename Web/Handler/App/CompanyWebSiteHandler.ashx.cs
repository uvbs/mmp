using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// 微网站 处理文件
    /// </summary>
    public class CompanyWebSiteHandler : IHttpHandler
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 微网站BLL
        /// </summary>
        BLLWebSite bll=new BLLWebSite();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 当前站点所有者信息
        /// </summary>
        //ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        /// <summary>
        /// 当前访问用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {


                //this.currWebSiteUserInfo = bllUser.GetUserInfo(bll.WebsiteOwner);
                currUserInfo = bllUser.GetCurrentUserInfo();
                //string action = context.Request["Action"];
                //switch (action)
                //{





                //}
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