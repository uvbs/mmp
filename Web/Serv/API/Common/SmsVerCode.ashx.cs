

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 发送短信验证码接口
    /// </summary>
    public class SmsVerCode : IHttpHandler, IReadOnlySessionState
    {

        /// <summary>
        /// 默认响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 短信业务逻辑Bll
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            string phone = context.Request["phone"];
            string smsContent = context.Request["smscontent"];
            string is_register = context.Request["is_register"];
            string is_reset = context.Request["is_reset"];
            string is_member = context.Request["is_member"];
            string content = context.Request["content"];
            List<string> blackIpList = new List<string>();
            blackIpList.Add("139.196.16.189");
            if (blackIpList.Contains(context.Request.UserHostAddress))
            {
                resp.errcode = 1;
                resp.errmsg = "Ip Invai";
                goto outoff;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号";
                goto outoff;
            }
            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 2;
                resp.errmsg = "手机号格式不正确";
                goto outoff;
            }
            if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
            {
                resp.errcode = 2;
                resp.errmsg = "手机格式不正确";
                goto outoff;
            }
            if (string.IsNullOrEmpty(smsContent))
            {
                resp.errcode = 1;
                resp.errmsg = "短信内容不能为空";
                goto outoff;
            }
            if (!smsContent.Contains("{{SMSVERCODE}}"))//验证码标签
            {
                resp.errcode = 1;
                resp.errmsg = "缺少标签{{SMSVERCODE}}";
                goto outoff;
            }
            if (is_register == "1")
            {
                BLLUser bllUser = new BLLUser();
                BLLJIMP.Model.UserInfo ouser = bllUser.GetUserInfoByPhone(phone, bllUser.WebsiteOwner);
                if (ouser != null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "手机号码已存在账号，请尝试找回密码";
                    goto outoff;
                }
            }
            if (is_reset == "1")
            {
                BLLUser bllUser = new BLLUser();
                BLLJIMP.Model.UserInfo ouser = bllUser.GetUserInfoByPhone(phone, bllUser.WebsiteOwner);
                if (ouser == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "手机号未注册";
                    goto outoff;
                }
                if (ouser.IsDisable == 1)
                {
                    resp.errcode = 1;
                    resp.errmsg = "账号已被禁用";
                    goto outoff;
                }
                if (is_member=="1" && ouser.MemberLevel == 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "您不是会员";
                    goto outoff;
                }
            }
            var lastSmsVerificationCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerificationCode != null)
            {

                if ((DateTime.Now - lastSmsVerificationCode.InsertDate).TotalSeconds < 60)
                {
                    resp.errcode = 3;
                    resp.errmsg = "验证码限制每60秒发送一次";
                    goto outoff;

                }

            }
            bool isSuccess = false;
            string verCode = new Random().Next(111111, 999999).ToString();
            string msg = "";
         
            smsContent = smsContent.Replace("{{SMSVERCODE}}", verCode);//替换验证码标签
            if (content == "1")
            {
                smsContent = "金融玩家欢迎你,验证码" + verCode;
            }
            string smsSignature = string.Format("{0}", bllSms.GetWebsiteInfoModelFromDataBase().SmsSignature);//短信签名

            bllSms.SendSmsVerificationCode(phone, smsContent, smsSignature, verCode, out isSuccess, out msg);
            if (isSuccess)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                //resp.sms_vercode = verCode;
            }
            else
            {
                resp.errcode = 4;
                resp.errmsg = string.Format("发送验证码失败{0}", msg);
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

        /// <summary>
        /// 默认响应模型
        /// </summary>
        public class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// 短信验证码
            /// </summary>
            //public string sms_vercode { get; set; }

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