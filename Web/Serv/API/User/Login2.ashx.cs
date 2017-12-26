using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// Login2 的摘要说明
    /// </summary>
    public class Login2 : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLUser bllUser = new BLLUser();


        protected BaseResponse apiResp = new BaseResponse();
        
        public void ProcessRequest(HttpContext context)
        {
            string userName = context.Request["userid"];
            string pwd = context.Request["pwd"];
            string applyStatus = context.Request["applystatus"];

            BLLJIMP.Model.API.forbes.Login apiResult = new BLLJIMP.Model.API.forbes.Login();

            UserInfo userInfo = bllUser.GetUserInfo(userName);
            if (userInfo == null)
            {
                userInfo = bllUser.Get<UserInfo>(string.Format(" Phone = '{0}' AND WebsiteOwner = '{1}' ", userName, bllUser.WebsiteOwner));
            }

            if (userInfo == null)
            {
                apiResp.msg = "用户不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context,apiResp);
                return;
            }

            if (!userInfo.Password.Equals(pwd))
            {
                apiResp.msg = "密码错误！";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (!userInfo.IsDisable.Equals(0))
            {
                apiResp.msg = "账号已被禁用！";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrEmpty(applyStatus))
            {
                if (userInfo.MemberApplyStatus == 1)
                {
                    apiResp.msg = "账号还在审核中！";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                if (userInfo.MemberApplyStatus == 2)
                {
                    apiResp.msg = "账号审核未通过！";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
            }

            if (userInfo != null)
            {
                if (DateTime.Now.ToString("yyyy-MM-dd") != userInfo.LastLoginDate.ToString("yyyy-MM-dd"))
                {
                    bllUser.AddUserScoreDetail(userInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.DayLogin), bllUser.WebsiteOwner, null, null);
                }
                bllUser.UpdateLoginInfo(userInfo);

                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserID] = userInfo.UserID;

                //绑定微信

                var openId = context.Session["currWXOpenId"] == null ? "" : context.Session["currWXOpenId"].ToString();

                //用户第一次登陆微信，绑定微信账号
                if (string.IsNullOrWhiteSpace(userInfo.WXOpenId) && !string.IsNullOrWhiteSpace(openId))
                {
                    if (bllUser.UpdateUserWxOpenId(userInfo.UserID, openId, bllUser.WebsiteOwner))
                    { }
                }
                

                apiResult.issuccess = true;
                apiResult.userid = userInfo.UserID;
                apiResult.headimg = this.bllUser.GetUserDispalyAvatar(userInfo);
                apiResult.userName = this.bllUser.GetUserDispalyName(userInfo);
                apiResult.avatar = this.bllUser.GetUserDispalyAvatar(userInfo);
                apiResult.phone = userInfo.Phone;
                apiResult.id = userInfo.AutoID;
                apiResult.score = userInfo.TotalScore;
                apiResult.im_token = userInfo.IMToken;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResult));
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