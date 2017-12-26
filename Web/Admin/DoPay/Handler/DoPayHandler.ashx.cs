using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// ScoreDefineHandler 的摘要说明
    /// </summary>
    public class DoPayHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser("");
        BllPay bllPay = new BllPay();
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
                    resp.Status = -1;
                    resp.Msg = "未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string action = context.Request["Action"];
                switch (action)
                {
                    case "PayConfig":
                        result = PayConfig(context);
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
        private string PayConfig(HttpContext context)
        {
            PayConfig payConfig = bllPay.GetPayConfig();
            payConfig = bllPay.ConvertRequestToModel<PayConfig>(payConfig);
            if (bllPay.Update(payConfig))
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