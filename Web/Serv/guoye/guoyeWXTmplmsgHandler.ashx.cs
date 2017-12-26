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
    public class guoyeWXTmplmsgHandler : IHttpHandler, IReadOnlySessionState
    {

        BLLWeixin bllWeixin = new BLLWeixin();
        BLLJuActivity bllActivity = new BLLJuActivity();
        DefaultResponse resp = new DefaultResponse();
        const string keyValueId = "7657";
        const string categoryId0 = "681";
        const string categoryId1 = "682";

        public void ProcessRequest(HttpContext context)
        {
            string accessToken = bllWeixin.GetAccessToken();
            //dealid   要约id
            //buyerid  买方id
            //buyername 买方名字
            //sellerid    卖方id
            //sellername 卖方名字
            //receiveropenid  微信接收方openid
            //receiverphone   微信接收方手机
            //identity  0是买方，1是卖方   
            //title 标题
            //detail 要约详情
            //time  发布时间，精确到分钟
            string title = context.Request["title"];
            string detail = context.Request["detail"];
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

            JuActivityInfo activity = new JuActivityInfo();
            activity.JuActivityID = Convert.ToInt32(bllActivity.GetGUID(TransacType.ActivityAdd));
            activity.ActivityName = title;
            activity.ActivityDescription = detail;
            activity.ArticleType = CommonPlatform.Helper.EnumStringHelper.ToString(ContentType.YaoYue);
            activity.K1 = context.Request["buyerid"];
            activity.K2 = context.Request["buyername"];
            activity.K3 = context.Request["sellerid"];
            activity.K4 = context.Request["sellername"];
            activity.K5 = context.Request["receiveropenid"];
            activity.K6 = context.Request["receiverphone"];
            activity.K7 = context.Request["dealid"];
            activity.K8 = context.Request["time"];

            if (string.IsNullOrWhiteSpace(activity.K8)) activity.K8 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            activity.K9 = context.Request["identity"];
            activity.CreateDate = DateTime.Now;
            activity.WebsiteOwner = bllWeixin.WebsiteOwner;
            if(context.Session[SessionKey.UserID] == null){
                activity.UserID = activity.WebsiteOwner;
            }
            else{
                activity.UserID = context.Session[SessionKey.UserID].ToString();
            }
            if(activity.K9 == "1"){
                activity.CategoryId = categoryId1;
            }
            else{
                activity.CategoryId = categoryId0;
            }

            if (bllActivity.Add(activity))
            {
                JToken sendData = JToken.Parse("{}");
                sendData["touser"] = activity.K5;
                sendData["url"] = "http://guoye.gotocloud8.net/customize/guoye/#/yaoyue/" + activity.JuActivityID;
                //SendData["url"] = "http://guoyetest.comeoncloud.net/customize/guoye/#/yaoyue/" + activity.JuActivityID;
                sendData["K1"] = "要约编号：" + activity.K7;
                string sender = "国烨网\n";
                if (activity.K9 == "1")
                {
                    sender += "买家：" + activity.K2;
                }
                else
                {
                    sender += "卖家：" + activity.K4;
                }
                sendData["K2"] = sender;
                sendData["K3"] = activity.K8;
                sendData["K4"] = "标题：" + activity.ActivityName + "\n要约内容：" + activity.ActivityDescription;
                resp.errmsg = bllWeixin.SendTemplateMessage(accessToken, keyValueId, sendData);
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "提交失败";
            }
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