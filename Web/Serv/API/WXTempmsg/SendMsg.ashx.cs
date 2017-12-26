using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZCJson.Linq;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.WXTempmsg
{
    /// <summary>
    /// 发送微信模板消息
    /// </summary>
    public class SendMsg : IHttpHandler, IReadOnlySessionState
    {
        BLLWeixin bllWeixin = new BLLWeixin();
        DefaultResponse resp = new DefaultResponse();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "消息模板编号不能为空";
                bllWeixin.ContextResponse(context, resp);
                return;
            }

            string openId = context.Request["openid"];
            string userId = context.Request["userid"];

            if (string.IsNullOrWhiteSpace(openId) && string.IsNullOrWhiteSpace(userId))
            {
                BLLJIMP.Model.UserInfo toUser = bllWeixin.GetCurrentUserInfo();
                if (toUser != null && !string.IsNullOrWhiteSpace(toUser.WXOpenId)) openId = toUser.WXOpenId;
            }
            else if (string.IsNullOrWhiteSpace(openId) && !string.IsNullOrWhiteSpace(userId))
            {
                BLLJIMP.Model.UserInfo toUser = bllWeixin.GetByKey<BLLJIMP.Model.UserInfo>("UserID", userId);
                if (toUser != null && !string.IsNullOrWhiteSpace(toUser.WXOpenId)) openId = toUser.WXOpenId;
            }

            if (string.IsNullOrWhiteSpace(openId))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "接收者openId不能为空";
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            string k1 = context.Request["k1"];
            string k2 = context.Request["k2"];
            string k3 = context.Request["k3"];
            string k4 = context.Request["k4"];
            string k5 = context.Request["k5"];
            string k6 = context.Request["k6"];
            string k7 = context.Request["k7"];
            string k8 = context.Request["k8"];
            string k9 = context.Request["k9"];
            string k10 = context.Request["k10"];
            JToken sendData = JToken.Parse("{}");
            sendData["touser"] = openId;

            if (!string.IsNullOrWhiteSpace(k1)) sendData["K1"] = k1;
            if (!string.IsNullOrWhiteSpace(k2)) sendData["K2"] = k2;
            if (!string.IsNullOrWhiteSpace(k3)) sendData["K3"] = k3;
            if (!string.IsNullOrWhiteSpace(k4)) sendData["K4"] = k4;
            if (!string.IsNullOrWhiteSpace(k5)) sendData["K5"] = k5;
            if (!string.IsNullOrWhiteSpace(k6)) sendData["K6"] = k6;
            if (!string.IsNullOrWhiteSpace(k7)) sendData["K7"] = k7;
            if (!string.IsNullOrWhiteSpace(k8)) sendData["K8"] = k8;
            if (!string.IsNullOrWhiteSpace(k9)) sendData["K9"] = k9;
            if (!string.IsNullOrWhiteSpace(k10)) sendData["K10"] = k10;

            string url = context.Request.Form["toUrl"];
            if (!string.IsNullOrWhiteSpace(url)) sendData["url"] = url;

            string accessToken = bllWeixin.GetAccessToken();
            resp.errmsg = bllWeixin.SendTemplateMessage(accessToken, id, sendData);
            if (!string.IsNullOrWhiteSpace(resp.errmsg))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                bllWeixin.ContextResponse(context, resp);
                return;
            }
            resp.isSuccess = true;
            bllWeixin.ContextResponse(context, resp);
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