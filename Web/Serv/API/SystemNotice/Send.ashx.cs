using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.SystemNotice
{
    /// <summary>
    /// Send 的摘要说明
    /// </summary>
    public class Send : BaseHandlerNeedLoginNoAction
    {
        BLLSystemNotice bll = new BLLSystemNotice();
        BLLUser bllUser = new BLLUser();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string type = context.Request["type"];
            string content = context.Request["content"];
            string articleName=context.Request["article_name"];
            string moduleName = "积分";
            if(!string.IsNullOrWhiteSpace(context.Request["module_name"])) moduleName = context.Request["module_name"];
            UserInfo toUser = bllUser.GetUserInfoByAutoID(Convert.ToInt32(id), bll.WebsiteOwner);
          
            if (toUser == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "通知对象未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }

            if (type == "2") // 收费短信通知
            {
                string sendNoticePrice = bllKeyValueData.GetDataVaule("SendNoticePrice", "1", bllKeyValueData.WebsiteOwner);
                if (string.IsNullOrWhiteSpace(sendNoticePrice))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    apiResp.msg = "未配置短信消费"+moduleName;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(toUser.Phone))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    apiResp.msg = "通知对象的手机未找到";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                int sendNoticePriceNum = Convert.ToInt32(sendNoticePrice);
                if (CurrentUserInfo.TotalScore < sendNoticePriceNum)
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    apiResp.msg = moduleName+"不足";
                    bll.ContextResponse(context, apiResp);
                    return;
                }

                bool sendSmsResult = false;

                string smsMsg = "您的";
                if (!string.IsNullOrEmpty(articleName))
                {
                    smsMsg += "《" + articleName + "》有最新提醒:"+content+"   提醒人："+bllUser.GetUserDispalyName(CurrentUserInfo);
                }
                

                string verCode = new Random().Next(111111, 999999).ToString();
                string msg = "";
                string smsSignature = string.Format("{0}", bllSms.GetWebsiteInfoModelFromDataBase().SmsSignature);//短信签名

                bllSms.SendSmsMisson(toUser.Phone, smsMsg, DateTime.Now.ToString(), smsSignature, out sendSmsResult, out msg);
                if (sendSmsResult)
                {
                    bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.SendMessage), bll.WebsiteOwner, (0 - sendNoticePriceNum), string.Format("短信通知消耗{0}{1}", sendNoticePriceNum, moduleName));

                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                    apiResp.msg = "短信发送成功";
                    apiResp.status = true;
                }
                else
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    apiResp.msg = "短信发送失败，" + msg;
                }
            }
            else
            {
                BLLJIMP.Model.SystemNotice notice = new BLLJIMP.Model.SystemNotice();
                notice.InsertTime = DateTime.Now;
                notice.UserId = CurrentUserInfo.UserID;
                notice.Receivers = toUser.UserID;
                notice.WebsiteOwner = bll.WebsiteOwner;
                notice.NoticeType = (int)BLLSystemNotice.NoticeType.Message;
                notice.SendType = 2;
                notice.Ncontent = bll.GetContentHtml(BLLSystemNotice.NoticeType.Message, CurrentUserInfo, null, content);
                notice.SerialNum = bll.GetGUID(TransacType.SendSystemNotice);
                bool addResult = bll.Add(notice);
                if (addResult)
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                    apiResp.msg = "通知发送成功";
                    apiResp.status = true;
                }
                else
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    apiResp.msg = "通知发送失败";
                }
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}