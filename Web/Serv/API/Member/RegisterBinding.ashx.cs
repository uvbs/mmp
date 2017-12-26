using Newtonsoft.Json.Linq;
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
    /// Register 的摘要说明
    /// </summary>
    public class RegisterBinding : IHttpHandler, IRequiresSessionState
    {
        BaseResponse apiResp = new BaseResponse();
        BLLUser bllUser = new BLLUser("");
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLSMS bllSms = new BLLSMS("");
        BLLWebSite bllWebSite = new BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request["code"];
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
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            #region 判断验证码是否正确
            SmsVerificationCode sms = bllSms.GetLastSmsVerificationCode(Phone);
            if (sms == null || sms.VerificationCode != code)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "验证码错误";
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, "0", null);
            List<string> defFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone" };

            #region 账号检查 未登录时检查已有账号
            CurrentUserInfo = bllUser.GetUserInfoByAllPhone(Phone);
            if (CurrentUserInfo != null)
            {
                List<string> tempFields = new List<string>() { "Phone1", "Phone2", "Phone3" };
                List<GetCompleteField.ResultField> resultList = new List<GetCompleteField.ResultField>();
                #region 取姓名
                TableFieldMapping AcountTrueNameField = listFieldList.FirstOrDefault(p => p.Field.Equals("TrueName"));

                if (AcountTrueNameField == null)
                {
                    resultList.Add(new GetCompleteField.ResultField { field = "TrueName", field_name = "姓名", type = "txt", no_null = 1, value = CurrentUserInfo.TrueName, read_only = 0 });
                }
                else
                {
                    resultList.Add(new GetCompleteField.ResultField { field = "TrueName", field_name = AcountTrueNameField.MappingName, type = "txt", no_null = AcountTrueNameField.FieldIsNull, value = CurrentUserInfo.TrueName, read_only = AcountTrueNameField.IsReadOnly });
                }
                #endregion
                #region 取手机
                if (!string.IsNullOrWhiteSpace(CurrentUserInfo.Phone1))
                {
                    TableFieldMapping AcountPhone1Field = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone1"));
                    if (AcountPhone1Field == null)
                    {
                        resultList.Add(new GetCompleteField.ResultField { field = "TrueName", field_name = "手机", type = "txt", no_null = 1, value = CurrentUserInfo.Phone1, read_only = 0 });
                    }
                    else
                    {
                        resultList.Add(new GetCompleteField.ResultField { field = "TrueName", field_name = AcountPhone1Field.MappingName, type = "txt", no_null = AcountPhone1Field.FieldIsNull, value = CurrentUserInfo.Phone1, read_only = AcountPhone1Field.IsReadOnly });
                    }
                }
                if (!string.IsNullOrWhiteSpace(CurrentUserInfo.Phone2))
                {
                    TableFieldMapping AcountPhone2Field = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone2"));
                    if (AcountPhone2Field == null)
                    {
                        resultList.Add(new GetCompleteField.ResultField { field = "Phone2", field_name = "手机", type = "txt", no_null = 1, value = CurrentUserInfo.Phone2, read_only = 0 });
                    }
                    else
                    {
                        resultList.Add(new GetCompleteField.ResultField { field = "Phone2", field_name = AcountPhone2Field.MappingName, type = "txt", no_null = AcountPhone2Field.FieldIsNull, value = CurrentUserInfo.Phone2, read_only = AcountPhone2Field.IsReadOnly });
                    }
                }
                if (!string.IsNullOrWhiteSpace(CurrentUserInfo.Phone3))
                {
                    TableFieldMapping AcountPhone3Field = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone3"));
                    if (AcountPhone3Field == null)
                    {
                        resultList.Add(new GetCompleteField.ResultField { field = "Phone3", field_name = "手机", type = "txt", no_null = 1, value = CurrentUserInfo.Phone3, read_only = 0 });
                    }
                    else
                    {
                        resultList.Add(new GetCompleteField.ResultField { field = "Phone3", field_name = AcountPhone3Field.MappingName, type = "txt", no_null = AcountPhone3Field.FieldIsNull, value = CurrentUserInfo.Phone3, read_only = AcountPhone3Field.IsReadOnly });
                    }
                }
                #endregion
                #region 取其他信息
                JObject tCurUser = JObject.FromObject(CurrentUserInfo);
                foreach (var item in listFieldList.Where(p => !defFields.Contains(p.Field) && !tempFields.Contains(p.Field)))
                {
                    if (tCurUser[item.Field] == null) continue;
                    if (string.IsNullOrWhiteSpace(tCurUser[item.Field].ToString())) continue;
                    string FieldType = string.IsNullOrWhiteSpace(item.FieldType) ? "txt" : item.FieldType;
                    resultList.Add(new GetCompleteField.ResultField { field = item.Field, field_name = item.MappingName, type = FieldType, no_null = item.FieldIsNull, value = tCurUser[item.Field].ToString(), read_only = item.IsReadOnly });
	            }
                #endregion

                apiResp.code = (int)APIErrCode.HaveHistoryAcount;
                apiResp.msg = "注册手机已存在账号";
                apiResp.result = new
                {
                    have_acount = true,
                    id = CurrentUserInfo.AutoID,
                    info_list = resultList
                };
                bllSms.ContextResponse(context, apiResp);
                return;
            }
            else
            {
                CurrentUserInfo = new UserInfo();
                string guidString = Guid.NewGuid().ToString();
                CurrentUserInfo.UserID = string.Format("WXUser{0}", guidString);//Guid
                CurrentUserInfo.Password = guidString.Substring(0,8);//Guid
                CurrentUserInfo.WXHeadimgurl = string.Format("http://{0}", context.Request.Url.Authority) + "/img/persion.png";
                CurrentUserInfo.WebsiteOwner = bllUser.WebsiteOwner;
                CurrentUserInfo.UserType = 2;
                CurrentUserInfo.WXOpenId = wxOpenId;
                CurrentUserInfo.Regtime = DateTime.Now;
                CurrentUserInfo.LastLoginDate = DateTime.Now;
            }
            #endregion


            //string oldPhone = CurrentUserInfo.Phone;
            CurrentUserInfo = bllTableFieldMap.ConvertRequestToModel<UserInfo>(CurrentUserInfo);
            //if(CurrentUserInfo.IsPhoneVerify == 1) CurrentUserInfo.Phone = oldPhone;

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
            foreach (var item in listFieldList.Where(p => p.FieldIsNull == 1 && !defFields.Contains(p.Field)).OrderBy(p => p.Sort))
            {
                if (jtCurUser[item.Field] == null) continue;
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
                if (CurrentUserInfo.AccessLevel < 1)
                {
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
            if (bllUser.Add(CurrentUserInfo))
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