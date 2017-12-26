using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.payload;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCAppPush
{
    public class GetuiPushHelper
    {
        private static String HOST = "http://sdk.open.api.getui.net/apiex.htm";

        private IGtPush push;


        public GetuiPushHelper(string appKey, string masterSecret)
        {
            push = new IGtPush("", appKey, masterSecret);
        }
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appKey"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        public string PushMessageToList(string appId,string appKey, string title, string text, string link, List<string> clientIds){

            TransmissionTemplate template = BuildTransmissionTemplate(appId, appKey, title, text, link);
            ListMessage message = new ListMessage();
            // 用户当前不在线时，是否离线存储,可选
            message.IsOffline = true;
            // 离线有效时间，单位为毫秒，可选
            //message.OfflineExpireTime = 1000 * 3600 * 12;
            message.Data = template;
            List<Target> targetList = new List<Target>();
            foreach (var clientId in clientIds)
            {
                targetList.Add(new Target
                {
                    appId = appId,
                    clientId = clientId
                });
            }
            String contentId = push.getContentId(message);
            String pushResult = push.pushMessageToList(contentId, targetList);
            return pushResult;
        }
        /// <summary>
        /// 构造通知模板
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appKey"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private TransmissionTemplate BuildTransmissionTemplate(string appId, string appKey, string title, string text, string link)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = appId;
            template.AppKey = appKey;
            //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionType = "1";

            JObject ob = new JObject();
            if (!string.IsNullOrWhiteSpace(link)) ob["title"] = title;
            if (!string.IsNullOrWhiteSpace(link)) ob["content"] = text;
            ob["type"] = 1;
            if (!string.IsNullOrWhiteSpace(link)) ob["link"] = link;
            //透传内容  
            template.TransmissionContent = JsonConvert.SerializeObject(ob);
            APNPayload apnpayload = new APNPayload();
            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();
            alertMsg.Body = text;
            //alertMsg.ActionLocKey = "ActionLocKey";
            //alertMsg.LocKey = "LocKey";
            //alertMsg.addLocArg("LocArg");
            //alertMsg.LaunchImage = "LaunchImage";
            //iOS8.2支持字段
            alertMsg.Title = title;
            //alertMsg.TitleLocKey = "TitleLocKey";
            //alertMsg.addTitleLocArg("TitleLocArg");

            apnpayload.AlertMsg = alertMsg;
            apnpayload.Badge = 1;
            apnpayload.ContentAvailable = 1;
            //apnpayload.Category = "";
            //apnpayload.Sound = "test1.wav";
            //apnpayload.addCustomMsg("link", link);
            template.setAPNInfo(apnpayload);
            return template;
        }
    }
}
