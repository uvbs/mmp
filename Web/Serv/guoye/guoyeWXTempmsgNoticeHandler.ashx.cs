using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZCJson.Linq;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.guoye
{
    /// <summary>
    /// 国烨
    /// </summary>
    public class guoyeWXTempmsgNoticeHandler : IHttpHandler, IReadOnlySessionState
    {

        BLLWeixin bllWeixin = new BLLWeixin();
        BLLJuActivity bllActivity = new BLLJuActivity();
        DefaultResponse resp = new DefaultResponse();
        const string keyValueId = "7648";
        const string categoryId = "683";

        public void ProcessRequest(HttpContext context)
        {
            string title = context.Request["title"];
            string detail = context.Request["detail"];
            string time = context.Request["time"];

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(detail))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "标题，内容不能为空";
                bllActivity.ContextResponse(context, resp);
                return;
            }
            string receiverOpenid = context.Request["receiveropenid"];
            if (string.IsNullOrWhiteSpace(receiverOpenid))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "接收者OpenId不能为空";
                bllActivity.ContextResponse(context, resp);
                return;
            }
            string url = context.Request.Form["toUrl"];
            string haveUrl = context.Request["haveUrl"];
            if (string.IsNullOrWhiteSpace(time)) time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            string accessToken = bllWeixin.GetAccessToken();

            JToken sendData = JToken.Parse("{}");
            sendData["touser"] = receiverOpenid;
            string activityId = string.Empty;
            if (!string.IsNullOrWhiteSpace(url))
            {
                sendData["url"] = url;
            }
            else if (haveUrl == "1")
            {
                activityId = bllActivity.GetGUID(TransacType.ActivityAdd);
                sendData["url"] = "http://guoye.gotocloud8.net/customize/guoye/#/tongzhi/" + activityId;
                //SendData["url"] = "http://guoyetest.comeoncloud.net/customize/guoye/#/tongzhi/" + ActivityID;
            }

            sendData["K1"] = "标题：" + title;
            sendData["K2"] = detail;
            sendData["K3"] = time;
            sendData["K4"] = "";
            resp.errmsg = bllWeixin.SendTemplateMessage(accessToken, keyValueId, sendData);
            if (!string.IsNullOrWhiteSpace(resp.errmsg))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                bllActivity.ContextResponse(context, resp);
                return;
            }

            if (string.IsNullOrWhiteSpace(url) && haveUrl == "1")
            {
                JuActivityInfo activity = new JuActivityInfo();
                activity.JuActivityID = Convert.ToInt32(activityId);
                activity.ActivityName = title;
                activity.ActivityDescription = detail;
                activity.ArticleType = CommonPlatform.Helper.EnumStringHelper.ToString(ContentType.Notice);
                activity.CategoryId = categoryId;
                activity.K8 = time;
                activity.K12 = context.Request["url"];
                activity.K13 = context.Request["haveUrl"];
                activity.WebsiteOwner = bllWeixin.WebsiteOwner;
                if (context.Session[SessionKey.UserID] == null)
                {
                    activity.UserID = activity.WebsiteOwner;
                }
                else
                {
                    activity.UserID = context.Session[SessionKey.UserID].ToString();
                }
                if (bllActivity.Add(activity))
                {
                    resp.isSuccess = true;
                }
                else
                {
                    resp.errmsg = "发送成功，但记录失败";
                    resp.errcode = (int)APIErrCode.OperateFail;
                }
            }
            else
            {
                resp.isSuccess = true;
            }
            bllActivity.ContextResponse(context, resp);
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