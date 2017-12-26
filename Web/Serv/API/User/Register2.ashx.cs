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
    /// Register2 的摘要说明
    /// </summary>
    public class Register2 : IHttpHandler, IRequiresSessionState
    {
        // <summary>
        /// 默认响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 用户处理逻辑BLL
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Expires = 0;
            string nickName = context.Request["nickname"];
            string phone = context.Request["username"];//手机号作为用户名
            string passWord = context.Request["password"];
            string passwordConfirm = context.Request["passwordconfirm"];
            string viewType = context.Request["view_type"];//0普通 1加密
            string isRepeat = context.Request["is_repeat_nickname"];//是否能注册相同的昵称
            if (string.IsNullOrEmpty(nickName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入昵称";
                goto outoff;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号码";
                goto outoff;
            }
            if (!string.IsNullOrEmpty(isRepeat) && isRepeat == "1")
            {
                if (bllUser.GetUserInfoByNickName(nickName) != null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "昵称重复";
                    goto outoff;
                }
            }
            //if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
            //{
            //    resp.errcode = (int)APIErrCode.PhoneFormatError;
            //    resp.errmsg = "手机格式不正确";
            //    goto outoff;
            //}
            string msg = "";

            string currWXOpenId = "";
            
            if (bllUser.RegByPhone(phone, passWord, passwordConfirm, out msg, nickName, currWXOpenId, viewType))
            {
                resp.errcode = 0;
                resp.errmsg = "注册成功";
                resp.isSuccess = true;
                UserInfo curUser = bllUser.GetUserInfoByPhone(phone);

                bllUser.AddUserScoreDetail(curUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ScoreDefineType.Register), bllUser.WebsiteOwner, null, null);

                context.Session[SessionKey.UserID] = curUser.UserID;
                context.Session[SessionKey.LoginStatu] = 1;
                context.Response.Cookies.Add(bllUser.CreateLoginCookie(curUser.UserID, curUser.WXOpenId, curUser.WXNickname));
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
        outoff:
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