using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.JubitIMP.Web.Handler;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// TutorApplyHander 的摘要说明
    /// </summary>
    public class OrderPayHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser();
        BllOrder bll = new BllOrder();
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
                    case "getOrderPayList":
                        result = getOrderPayList(context);
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
        /// 支付订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getOrderPayList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string type = context.Request["type"];
            string ex1 = context.Request["ex1"];
            string ex2 = context.Request["ex2"];

            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;

            int totalCount = 0;
            var dataList = bll.GetOrderPayList(rows, page,"1", type, ex1, ex2, out totalCount,bll.WebsiteOwner);

            var data = from p in dataList
                       select new {
                           Ex2 = string.IsNullOrWhiteSpace(p.Ex2)?p.UserId:p.Ex2,
                           Subject = p.Subject,
                           Total_Fee = p.Total_Fee,
                           Ex1 = (p.Type=="2" && p.Ex1=="1") ? "带发票":"",
                           Status = p.Status
                       };

            return Common.JSONHelper.ObjectToJson(new
            {
                rows = data,
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