using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();

        public void ProcessRequest(HttpContext context)
        {
            UserInfo nUser = new UserInfo();
            nUser = bllTableFieldMap.ConvertRequestToModel<UserInfo>(nUser);
            nUser.UserID = string.Format("PCUser{0}", Guid.NewGuid().ToString());//Guid
            nUser.Password = ZentCloud.Common.Rand.Str_char(12);
            nUser.UserType = 2;
            nUser.WebsiteOwner = bllTableFieldMap.WebsiteOwner;
            nUser.LastLoginDate = DateTime.Now;


            List<TableFieldMapping> formField = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, context.Request["mapping_type"]);
            formField = formField.Where(p => p.IsReadOnly == 0 && p.IsDelete == 0 && p.Field != "AutoID" && p.Field != "UserID").ToList();

            List<string> defFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone", "WebsiteOwner" };

            JObject jtCurUser = JObject.FromObject(nUser);
            List<JProperty> listPropertys = jtCurUser.Properties().ToList();
            foreach (var item in formField.Where(p => p.FieldIsNull == 1 && !defFields.Contains(p.Field)).OrderBy(p => p.Sort))
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

            if (bllTableFieldMap.Add(nUser))
            {
                if (!string.IsNullOrEmpty(nUser.TagName))
                {
                    foreach (var tag in nUser.TagName.Split(','))
                    {
                        if (bllUser.GetCount<ZentCloud.BLLJIMP.Model.MemberTag>(string.Format(" WebsiteOwner='{0}' And TagName='{1}' And TagType='Member'", bllUser.WebsiteOwner, tag)) == 0)
                        {

                            ZentCloud.BLLJIMP.Model.MemberTag model = new BLLJIMP.Model.MemberTag();
                            model.CreateTime = DateTime.Now;
                            model.WebsiteOwner = bllUser.WebsiteOwner;
                            model.TagType = "Member";
                            model.TagName = tag;
                            model.Creator = currentUserInfo.UserID;
                            if (!bllUser.Add(model))
                            {
                                apiResp.msg = "新增标签失败";
                                apiResp.code = (int)APIErrCode.OperateFail;
                                bllTableFieldMap.ContextResponse(context, apiResp);
                            }

                        }
                    }
                }
                apiResp.status = true;
                apiResp.msg = "新增完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "新增失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllTableFieldMap.ContextResponse(context, apiResp);
        }

    }
}