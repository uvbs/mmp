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
    /// OAuthBindPhone 的摘要说明
    /// </summary>
    public class OAuthBindPhone : IHttpHandler, IRequiresSessionState
    {
        protected BaseResponse apiResp = new BaseResponse();
        protected BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            //string openid = context.Request["openid"];
            string phone = context.Request["phone"];
            string code = context.Request["code"];
            string msg ="";
            if (!bllUser.SmsLogin(phone, code, out msg))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = msg;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            string unionid = context.Request["unionid"];
            string websiteOwner = bllUser.WebsiteOwner;
            UserInfo user = bllUser.GetUserInfoByPhone(phone, websiteOwner);
            if (user == null){
                user = new UserInfo();
                user.UserID =
                user.UserID = string.Format("Phone{0}", Guid.NewGuid().ToString());
                user.Phone = phone;
                user.Password = ZentCloud.Common.Rand.Str_char(12);
                user.UserType = 2;
                user.WebsiteOwner = websiteOwner;
                user.Regtime = DateTime.Now;
                user.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            }
            string oUnionid = user.WXUnionID;
            string oNickname = user.WXNickname;
            bool hasOther = (user.AutoID!=0 && oUnionid != unionid);

            string password = context.Request["password"];
            string city = context.Request["city"];
            string country = context.Request["country"];
            string headimgurl = context.Request["headimgurl"];
            string nickname = context.Request["nickname"];
            string privilege = context.Request["privilege"];
            string province = context.Request["province"];
            string sex = context.Request["sex"];
            user.Password = password.Trim();
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = ZentCloud.Common.Rand.Str_char(6);
            }
            user.WXCity = city;
            user.WXCountry = country;
            user.WXHeadimgurl = headimgurl;
            user.WXNickname = nickname;
            user.WXPrivilege = ZentCloud.Common.JSONHelper.ObjectToJson(privilege);
            user.WXProvince = province;
            user.WXSex = Convert.ToInt32(sex);
            user.WXUnionID = unionid;

            user.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
            user.LastLoginDate = DateTime.Now;
            user.LoginTotalCount = user.LoginTotalCount + 1;

            bool status = false;
            if (user.AutoID == 0){
                //更新微信授权信息
                status =bllUser.Add(user);
            }
            else
            {
                status = bllUser.Update(user);
            }

            if (status)
            {
                if (hasOther)
                {
                    BLLLog bllLog = new BLLLog();
                    string rmk = string.Format("账号({0})绑定信息变更，微信UnionID：[{1}-{2}]，昵称：[{3}-{4}]", user.AutoID, oUnionid, unionid, oNickname, nickname);
                    bllLog.Add(ZentCloud.BLLJIMP.Enums.EnumLogType.OAuthBind, ZentCloud.BLLJIMP.Enums.EnumLogTypeAction.SignIn, user.UserID, rmk);
                }

                context.Session[SessionKey.UserID] = user.UserID;
                context.Session[SessionKey.LoginStatu] = 1;
                context.Response.Cookies.Add(bllUser.CreateLoginCookie(user.UserID, user.WXOpenId, user.WXNickname));

                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "授权绑定手机完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "授权绑定手机失败";
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