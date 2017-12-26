using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{

    /// <summary>
    /// 提交短信提醒接口
    /// </summary>
    public class SmsMission : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 短信业务逻辑Bll
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {

            string phone = context.Request["phone"];
            string smsContent = context.Request["sms_content"];
            string sendTime = context.Request["send_time"];
            string result = "false";
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "phone 参数不能为空";
                goto outoff;
            }
            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 2;
                resp.errmsg = "手机号格式不正确";
                goto outoff;
            }
            if (string.IsNullOrEmpty(smsContent))
            {
                resp.errcode = 1;
                resp.errmsg = "sms_content 参数不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(sendTime))
            {
                resp.errcode = 1;
                resp.errmsg = "send_time 参数不能为空";
                goto outoff;
            }
            bool isSuccess = false;
            string msg = "";
            string smsSignature = bllSms.GetWebsiteInfoModelFromDataBase().SmsSignature;//短信签名
            if (string.IsNullOrEmpty(smsSignature))
            {
                smsSignature = "至云";
            }
            bllSms.SendSmsMisson(phone, smsContent, sendTime, smsSignature, out isSuccess, out msg);
            if (isSuccess)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
               
            }
            else
            {
                resp.errcode = 4;
                resp.errmsg = string.Format("提交失败,错误码{0}", msg);
            }

        outoff:
            result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }


        }


    }
}