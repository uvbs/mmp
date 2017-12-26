using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.GreetingCard
{
    /// <summary>
    /// JuActivityHandler 的摘要说明
    /// </summary>
    public class JuActivityHandler : IHttpHandler, IRequiresSessionState
    {
        BLLJuActivity bllJuActivity=new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";

            try
            {
                string action = context.Request["Action"];
                switch (action)
                {
                    case "QueryJuActivityForWeb":
                        result = QueryJuActivityForWeb(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            context.Response.Write(result);
        }



        /// <summary>
        ///查询聚活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityForWeb(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserID"];
            string isToJubitActivity = context.Request["IsToJubitActivity"];
            string activityName = context.Request["ActivityName"];
            string isSignUpJubit = context.Request["IsSignUpJubit"];
            string articleType = context.Request["ArticleType"];
            int totalCount = 0;
            List<JuActivityInfo> dataList = this.bllJuActivity.QueryJuActivityData(
                null, out totalCount, null, null, null, null, activityName, pageIndex, pageSize,
                null, null, articleType, bllJuActivity.WebsiteOwner);
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].ActivityDescription = null;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new {
                rows = dataList,
                total = totalCount
            });
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