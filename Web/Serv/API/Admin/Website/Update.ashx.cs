using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Website
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 设置短信余额提醒
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string smsRemindValue=context.Request["sms_remind_value"];
            string smsRemindPhones=context.Request["sms_remind_phones"];
            string smsRemindFrequency = context.Request["sms_remind_frequency"];

            var website = bllWebsite.GetWebsiteInfoModelFromDataBase();

            website.SmsAccountRemindValue = Convert.ToInt32(smsRemindValue);
            website.SmsAccountRemindPhones = smsRemindPhones;
            website.SmsAccountRemindFrequency = Convert.ToInt32(smsRemindFrequency);

            if (bllWebsite.Update(website))
            {
                apiResp.msg = "设置成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "设置出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        
    }
}