using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Website
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {

            var website = bllUser.GetWebsiteInfoModelFromDataBase();

            apiResp.status = true;
            int account=0;
            string msg=string.Empty;
            bool smsAccount=bllSms.GetSmsDeposit(bllSms.WebsiteOwner, out account, out msg);
            apiResp.result = new {
                sms_remind_value = website.SmsAccountRemindValue,
                sms_remind_phones=website.SmsAccountRemindPhones,
                sms_remind_frequency = website.SmsAccountRemindFrequency,
                sms_account = smsAccount?account:0,
                websiteowner=website.WebsiteOwner,
                sms_is_open = smsAccount
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        
    }
}