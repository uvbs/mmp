using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// AboutHandler 的摘要说明
    /// </summary>
    public class AboutHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bll = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        UserInfo currentUserInfo;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.Status = (int)APIErrCode.UserIsNotLogin;
                    resp.Msg = "用户未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string Action = context.Request["Action"];
                switch (Action)
                {
                    case "EditAbout":
                        result = EditAbout(context);
                        break;
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

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditAbout(HttpContext context)
        {

            JuActivityInfo model = new JuActivityInfo();
            model.JuActivityID = Convert.ToInt32(context.Request["JuActivityID"]);
            if (model.JuActivityID == 0)
            {
                resp.Status = -1;
                resp.Msg = "内容不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            model = bll.GetJuActivity(model.JuActivityID);

            if (model == null)
            {
                resp.Status = -1;
                resp.Msg = "内容不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            model.ActivityDescription = context.Request["ActivityDescription"];
            if (bll.PutArticle(model))
            {
                resp.Status = 1;
                resp.Msg = "提交成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "提交失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
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