using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// RegisterByCompany 的摘要说明  公司注册
    /// </summary>
    public class RegisterByCompany : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();

        /// <summary>
        /// 用户 逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        public void ProcessRequest(HttpContext context)
        {
            string company = context.Request["company"];//公司名称
            string licence = context.Request["licence"];//营业执照
            string password = context.Request["pwd"];//密码
            string confirmPassword = context.Request["confirm_pwd"];
            if (string.IsNullOrEmpty(company))
            {
                apiResp.msg = "请输入公司名称";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(licence))
            {
                apiResp.msg = "请上传你的营业执照";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
            }
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                apiResp.msg = "请输入密码";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (password != confirmPassword)
            {
                apiResp.msg = "两次输入的密码不一致";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (password.Length < 6)
            {
                apiResp.msg = "密码长度不少于6位";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            UserInfo userInfo = bllUser.GetUserInfoByCompany(company);
            if (userInfo != null)
            {
                apiResp.msg = "公司名称重复";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                bllUser.ContextResponse(context, apiResp);
                return;
            }


            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(password))
            {
                apiResp.msg = "密码只能为字母或者数字";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            string userId = "";
            try
            {
                userId = "JRWJ" + bllUser.GetNewGUID(bllUser.WebsiteOwner, "Company", 6);
            }
            catch (Exception ex)
            {
                apiResp.msg = "生成id出错:" + ex.Message;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.RegisterFailure;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            userInfo = new UserInfo();
            userInfo.UserID = userId;
            userInfo.Password = password;
            userInfo.UserType = 6;//6公司  2普通用户
            userInfo.WebsiteOwner = bllUser.WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.Company = company;
            userInfo.TrueName = company;
            userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.LoginTotalCount = 1;
            userInfo.MemberApplyStatus = 1;//待审核
            userInfo.Ex3 = licence;//营业执照


            try
            {
                if (bllUser.Add(userInfo))
                {
                    bllUser.UpdateNewGUID(bllUser.WebsiteOwner, "Company");
                    apiResp.msg = "注册成功，您的公司账号为：" + userInfo.UserID;
                    context.Session[SessionKey.UserID] = userInfo.UserID;
                    context.Session[SessionKey.LoginStatu] = 1;
                    apiResp.status = true;
                }
                else
                {
                    apiResp.msg = "注册失败";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.RegisterFailure;
                }
            }
            catch (Exception ex)
            {
                apiResp.msg = "添加账号出错："+ex.Message;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.RegisterFailure;
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