using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Outlets.Comm
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            JuActivityInfo ninfo = bll.GetByKey<JuActivityInfo>("JuActivityID",context.Request["id"]);
            if (ninfo == null)
            {
                apiResp.msg = "原记录未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
            }
            #region 字段检查

            ArticleCategoryTypeConfig typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, ninfo.ArticleType);
            if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
            {
                ninfo.UserLongitude = context.Request["UserLongitude"];
                ninfo.UserLatitude = context.Request["UserLatitude"];
            }

            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_JuActivityInfo", ninfo.ArticleType, null, "0", null);
            List<string> DefFields = new List<string>() { "JuActivityID" };
            JObject jtCurUser = JObject.FromObject(ninfo);
            List<string> listPropertys = jtCurUser.Properties().Select(p => p.Name).ToList();
            foreach (var item in listFieldList.Where(p => !DefFields.Contains(p.Field) && listPropertys.Contains(p.Field)).OrderBy(p => p.Sort))
            {
                string nValue = context.Request[item.Field];
                string oValue = jtCurUser[item.Field].ToString();
                if (!string.IsNullOrWhiteSpace(oValue) && item.IsReadOnly == 1) continue;
                if (item.FieldIsNull == 1 && string.IsNullOrWhiteSpace(nValue))
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
                        if (!MyRegex.IsNumber(nValue))
                        {
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                            bllTableFieldMap.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    if (item.FormatValiFunc == "phone")//email检查
                    {
                        if (!MyRegex.PhoneNumLogicJudge(nValue))
                        {
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = string.Format("{0}格式不正确", item.MappingName);
                            bllTableFieldMap.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    if (item.FormatValiFunc == "email")//email检查
                    {
                        if (!MyRegex.EmailLogicJudge(nValue))
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
                        System.Text.RegularExpressions.Match match = regUrl.Match(nValue);
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

               ninfo = bll.ConvertToModel<JuActivityInfo>(ninfo, item.Field, nValue);
            }
            #endregion

            if (bll.Update(ninfo))
            {
                apiResp.status = true;
                apiResp.msg = "提交成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }

            bll.ContextResponse(context, apiResp);
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