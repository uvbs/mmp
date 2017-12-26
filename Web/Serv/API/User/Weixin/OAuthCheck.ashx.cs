using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Weixin
{
    /// <summary>
    /// OAuthCheck 的摘要说明
    /// </summary>
    public class OAuthCheck : IHttpHandler, IRequiresSessionState
    {
        protected BaseResponse apiResp = new BaseResponse();
        protected BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            //string openid = context.Request["openid"];
            string unionid = context.Request["unionid"];
            string websiteOwner = bllUser.WebsiteOwner;
            UserInfo user = bllUser.GetUserInfoByWXUnionID(unionid, websiteOwner);
            //if (user == null) user = bllUser.GetUserInfoByOpenId(openid, websiteOwner); 
            if (user == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "该微信未注册账号";
            }
            else
            {
                //if (string.IsNullOrWhiteSpace(user.WXOpenId)) user.WXOpenId = openid;
                //if (string.IsNullOrWhiteSpace(user.WXUnionID)) user.WXUnionID = unionid;

                string city = context.Request["city"];
                string country = context.Request["country"];
                string headimgurl = context.Request["headimgurl"];
                string nickname = context.Request["nickname"];
                string privilege = context.Request["privilege"];
                string province = context.Request["province"];
                string sex = context.Request["sex"];
                user.WXCity = city;
                user.WXCountry = country;
                user.WXHeadimgurl = headimgurl;
                user.WXNickname = nickname;
                user.WXPrivilege = ZentCloud.Common.JSONHelper.ObjectToJson(privilege);
                user.WXProvince = province;
                user.WXSex = Convert.ToInt32(sex);

                context.Session[SessionKey.UserID] = user.UserID;
                context.Session[SessionKey.LoginStatu] = 1;

                context.Response.Cookies.Add(bllUser.CreateLoginCookie(user.UserID, user.WXOpenId, user.WXNickname));

                //更新微信授权信息
                bllUser.Update(user);
                //更新最后登录信息
                bllUser.UpdateLastLoginInfo(user.UserID, "", "", "", bllUser.WebsiteOwner);
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "授权登陆完成";
            }
            bllUser.ContextResponse(context, apiResp);
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