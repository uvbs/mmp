using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Member
{
    /// <summary>
    /// Binding 的摘要说明
    /// </summary>
    public class Binding: IHttpHandler, IRequiresSessionState
    {
        BaseResponse apiResp = new BaseResponse();
        BLLUser bllUser = new BLLUser("");
        BLLWebSite bllWebSite = new BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string Phone = context.Request["Phone"];
            UserInfo CurrentUserInfo = bllUser.GetCurrentUserInfo();
            #region 检查是否已登录
            if (CurrentUserInfo != null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "本功能仅供新用户使用";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            #region 检查是否微信服务号
            if (context.Session["currWXOpenId"] == null)
            {
                apiResp.code = (int)APIErrCode.UserIsNotLogin;
                apiResp.msg = "本功能仅供微信服务号使用";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            string wxOpenId = context.Session["currWXOpenId"].ToString();
            CurrentUserInfo = bllUser.GetUserInfoByOpenId(wxOpenId);
            if (CurrentUserInfo != null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "OpenId已被绑定";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            #region 判断手机格式
            if (!MyRegex.PhoneNumLogicJudge(Phone))
            {
                apiResp.code = (int)APIErrCode.PhoneFormatError;
                apiResp.msg = "手机格式错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            #region 判断手机是否已被使用
            UserInfo model = bllUser.GetUserInfoByPhone(Phone);
            if (model != null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "手机号码已被其他账号使用，请联系管理员";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            CurrentUserInfo = bllUser.GetUserInfoByAutoID(Convert.ToInt32(id), bllUser.WebsiteOwner);
            if (CurrentUserInfo == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "绑定账号未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(CurrentUserInfo.WXOpenId) && CurrentUserInfo.WXOpenId != wxOpenId)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "账号已有其他微信绑定";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            List<string> pmsString = new List<string>();

            pmsString.Add(string.Format("Phone='{0}'", Phone));
            pmsString.Add(string.Format("WXOpenId='{0}'", wxOpenId));
            pmsString.Add(string.Format("IsPhoneVerify='{0}'", 1));
            CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            if (nWebsiteConfig.MemberStandard == 3)
            {
                pmsString.Add(string.Format("MemberApplyStatus='{0}'", 1));
                pmsString.Add(string.Format("MemberApplyTime='{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            else
            {
                if (CurrentUserInfo.AccessLevel < 1)
                {
                    CurrentUserInfo.AccessLevel = 1;
                    CurrentUserInfo.MemberStartTime = DateTime.Now;
                }
                pmsString.Add(string.Format("AccessLevel='{0}'", CurrentUserInfo.AccessLevel));
                pmsString.Add(string.Format("MemberStartTime='{0}'", CurrentUserInfo.MemberStartTime.ToString("yyyy-MM-dd HH:mm:ss")));
                //CurrentUserInfo.MemberApplyStatus = 9;
            }

            if (bllUser.Update(new UserInfo(),
                ZentCloud.Common.MyStringHelper.ListToStr(pmsString,"",","),
                string.Format("AutoID={0}",CurrentUserInfo.AutoID.ToString()))>0)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "提交完成";

                context.Session[ZentCloud.Common.SessionKey.UserID] = CurrentUserInfo.UserID;
                context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "提交失败";
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