using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Member
{
    /// <summary>
    /// PostCompleteUserInfo 的摘要说明
    /// </summary>
    public class PostCompleteUserInfo : BaseHandlerNoAction
    {
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLSMS bllSms = new BLLSMS("");
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWebSite = new BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request["code"];
            string Phone = context.Request["Phone"];

            string wxOpenId; 

            UserInfo CurrentUserInfo = bllUser.GetCurrentUserInfo();

            #region 判断手机格式
            if (!MyRegex.PhoneNumLogicJudge(Phone))
            {
                apiResp.code = (int)APIErrCode.PhoneFormatError;
                apiResp.msg = "手机格式错误";
                bllTableFieldMap.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            #region 判断验证码是否正确
            SmsVerificationCode sms = bllSms.GetLastSmsVerificationCode(Phone);
            if (sms ==null || sms.VerificationCode != code)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "验证码错误";
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            #region 账号检查 未登录时检查已有账号
            if (CurrentUserInfo == null)
            {
                if (context.Session["currWXOpenId"] == null){
                    apiResp.code = (int)APIErrCode.UserIsNotLogin;
                    apiResp.msg = "请先登录";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
                wxOpenId = context.Session["currWXOpenId"].ToString();
                UserInfo curUser = bllUser.GetUserInfoByOpenId(wxOpenId);
                if (curUser != null){
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "微信已绑定有账号";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
                curUser = bllUser.GetUserInfoByAllPhone(Phone);
                if (curUser != null)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "微信已绑定有账号";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }

            }
            #endregion

            #region 判断手机是否已被使用
            UserInfo model = bllUser.GetUserInfoByPhone(Phone);
            if (model != null)
            {
                if (model.UserID != CurrentUserInfo.UserID)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "手机号码已被其他账号使用，请联系管理员";
                    bllSms.ContextResponse(context, apiResp);
                    return;
                }
            }
            #endregion

            //string oldPhone = CurrentUserInfo.Phone;
            CurrentUserInfo = bllTableFieldMap.ConvertRequestToModel<UserInfo>(CurrentUserInfo);
            //if(CurrentUserInfo.IsPhoneVerify == 1) CurrentUserInfo.Phone = oldPhone;

            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByTableName(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo");

            List<string> DefFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone"};

            #region 默认信息检查 姓名
            TableFieldMapping TrueNameField = listFieldList.FirstOrDefault(p => p.Field.Equals("TrueName"));
            if ((TrueNameField == null || TrueNameField.FieldIsNull == 1) && string.IsNullOrWhiteSpace(CurrentUserInfo.TrueName))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "请完善姓名";
                bllTableFieldMap.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            JObject jtCurUser = JObject.FromObject(CurrentUserInfo);
            List<JProperty> listPropertys = jtCurUser.Properties().ToList();
            foreach (var item in listFieldList.Where(p => p.FieldIsNull == 1 && !DefFields.Contains(p.Field)).OrderBy(p => p.Sort))
            {
                if (!listPropertys.Exists(p => p.Name.Equals(item.Field))) continue;
                if (string.IsNullOrWhiteSpace(jtCurUser[item.Field].ToString()))
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "请完善" + item.MappingName;
                    bllTableFieldMap.ContextResponse(context, apiResp);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(item.FormatValiFunc))
                {
                    #region 检查数据格式
                    //检查数据格式
                    if (item.FormatValiFunc == "number")
                    {
                        if (!MyRegex.IsNumber(jtCurUser[item.Field].ToString()))
                        {
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                            bllTableFieldMap.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    if (item.FormatValiFunc == "phone")//email检查
                    {
                        if (!MyRegex.PhoneNumLogicJudge(jtCurUser[item.Field].ToString()))
                        {
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                            bllTableFieldMap.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    if (item.FormatValiFunc == "email")//email检查
                    {
                        if (!MyRegex.EmailLogicJudge(jtCurUser[item.Field].ToString()))
                        {
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                            bllTableFieldMap.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    if (item.FormatValiFunc == "url")//url检查
                    {
                        System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                        System.Text.RegularExpressions.Match match = regUrl.Match(jtCurUser[item.Field].ToString());
                        if (!match.Success)
                        {
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                            bllTableFieldMap.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    #endregion
                }
            }
            CurrentUserInfo.IsPhoneVerify = 1;
            CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            if (nWebsiteConfig.MemberStandard == 2)
            {
                if (CurrentUserInfo.AccessLevel < 1) {
                    CurrentUserInfo.AccessLevel = 1;
                    CurrentUserInfo.MemberStartTime = DateTime.Now;
                }
                //CurrentUserInfo.MemberApplyStatus = 9;
            }
            else if (nWebsiteConfig.MemberStandard == 3)
            {
                CurrentUserInfo.MemberApplyStatus = 1;
                CurrentUserInfo.MemberApplyTime = DateTime.Now;
            }
            if (bllUser.Update(CurrentUserInfo))
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "提交完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "提交失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}