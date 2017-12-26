using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZCJson.Linq;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// Register 的摘要说明
    /// </summary>
    public class Register : IHttpHandler, IRequiresSessionState
    {
        DefaultResponse resp = new DefaultResponse();
        BLLUser bllUser = new BLLUser("");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;

            string name = context.Request["name"];
            string email = context.Request["email"];
            string phone = context.Request["phone"];
            string company = context.Request["company"];
            string pwd = context.Request["pwd"];
            string hasCode = context.Request["hasCode"]; //0时没有验证码
            string verCode = context.Request["vercode"];
            string currWXOpenId = "";
            if (context.Session["currWXOpenId"] != null)
            {
                currWXOpenId = context.Session["currWXOpenId"].ToString();
            }
            if (string.IsNullOrEmpty(name))
            {
                resp.errcode = (int)APIErrCode.UserNameIsEmpty;
                resp.errmsg = "请输入姓名";
                bllUser.ContextResponse(context, resp);
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入邮箱";

                bllUser.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入密码";

                bllUser.ContextResponse(context, resp);
                return;
            }

            if (!ZentCloud.Common.MyRegex.EmailLogicJudge(email))
            {
                resp.errcode = 5;
                resp.errmsg = "邮箱格式不正确";

                bllUser.ContextResponse(context, resp);
                return;
            }
            if (phone != null)
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "请输入手机";

                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
                {
                    resp.errcode = (int)APIErrCode.PhoneFormatError;
                    resp.errmsg = "手机格式不正确";

                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (bllUser.GetUserInfoByPhone(phone) != null)
                {
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "此手机号已被注册，请输入别的手机号";

                    bllUser.ContextResponse(context, resp);
                    return;
                }
            }

            if (hasCode != "0")
            {
                if (string.IsNullOrWhiteSpace(verCode))
                {
                    resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "请输入验证码";

                    bllUser.ContextResponse(context, resp);
                    return;
                }
                var serverCheckCode = context.Session["CheckCode"];
                if (!verCode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase))
                {

                    resp.errcode = (int)APIErrCode.CheckCodeErr;
                    resp.errmsg = "验证码错误";

                    bllUser.ContextResponse(context, resp);
                    return;
                }
            }
            bool isSyncPass = true;
            string basePath = string.Format("http://{0}{1}", context.Request.Url.Host, context.Request.Url.Port != 80 ? ":" + context.Request.Url.Port.ToString() : "");

            #region 同步国烨账号注册检查
            if (bllUser.WebsiteOwner == "guoye")
            {
                isSyncPass = false;
                Dictionary<string, string> param = new Dictionary<string,string>();
                param.Add("email",email);
                param.Add("phone",phone);
                param.Add("pwd",pwd);
                param.Add("openid", currWXOpenId);
                param.Add("name",name);
                param.Add("company",company);
                using (HttpWebResponse wr = ZentCloud.Common.HttpInterFace.CreatePostHttpResponse(
                    "http://www.chinayie.com/api/register", param, null, null, Encoding.UTF8,null))
	            {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        JToken jto = JToken.Parse(sr.ReadToEnd());
                        if (jto["result"] != null && jto["result"].ToString().ToLower() == "true")
                        {
                            isSyncPass = true;
                        }
                        else if (jto["result"] != null && jto["msg"]!=null && jto["result"].ToString().ToLower() == "false")
                        {
                            
                            resp.errcode = (int)APIErrCode.RegisterFailure;
                            resp.errmsg = jto["msg"].ToString();
                            bllUser.ContextResponse(context, resp);
                            return;
                            
                        }
                    }
                }
                if (isSyncPass)
                {
                    resp.errcode = (int)APIErrCode.IsSuccess;
                    resp.errmsg = "注册成功";
                    resp.isSuccess = true;
                }
                else
                {
                    resp.errcode = (int)APIErrCode.RegisterFailure;
                    resp.errmsg = "注册失败";
                }
                bllUser.ContextResponse(context, resp);
                return;
            }
            #endregion

            UserInfo regUser = new UserInfo();
            regUser = bllUser.GetUserInfoByOpenId(currWXOpenId);
            if (regUser != null)
            {
                resp.errcode = (int)APIErrCode.RegisterFailure;
                resp.errmsg = "已经注册过";
                bllUser.ContextResponse(context, resp);
                return;
            }

            regUser = new UserInfo();
            regUser.TrueName = name;
            regUser.WXOpenId = currWXOpenId;
            regUser.WXHeadimgurl = basePath + "/img/persion.png";
            regUser.Email = email;
            regUser.Phone = phone;
            regUser.Company = company;
            regUser.Password = pwd;
            regUser.UserID = string.Format("ZYUser{0}{1}", StringHelper.GetDateTimeNum(), Rand.Str(5));
            regUser.WebsiteOwner = bllUser.WebsiteOwner;
            regUser.UserType = 2;
            regUser.Regtime = DateTime.Now;
            regUser.LastLoginDate = DateTime.Now;

            if (isSyncPass && bllUser.Add(regUser))
            {
                context.Session[SessionKey.UserID] = regUser.UserID;
                context.Session[SessionKey.LoginStatu] = 1;

                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.errmsg = "注册成功";
                resp.isSuccess = true;
                resp.returnObj = new
                {
                    session_id = context.Session.SessionID,
                    openid = regUser.WXOpenId
                };
            }
            else
            {
                resp.errcode = (int)APIErrCode.RegisterFailure;
                resp.errmsg = "注册失败";
            }
            bllUser.ContextResponse(context, resp);
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