using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Outlets.Comm
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            Dictionary<string, string> equals = new Dictionary<string,string>();
            Dictionary<string, string> contains = new Dictionary<string,string>();
            Dictionary<string, string> keywords = new Dictionary<string,string>();
            List<string> colFields = new List<string>() { "JuActivityID" }; 
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string type = context.Request["type"];
            string onlyLngLatIsNull = context.Request["OnlyLngLatIsNull"];
            ArticleCategoryTypeConfig typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, type);

            if (!string.IsNullOrWhiteSpace(typeConfig.ListFields))
            {
                foreach (var item in typeConfig.ListFields.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(context.Request[item])) equals.Add(item, context.Request[item]);
                }
            }
            if (!string.IsNullOrWhiteSpace(typeConfig.EditFields))
            {
                foreach (var item in typeConfig.EditFields.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(context.Request[item])) contains.Add(item, context.Request[item]);
                }
            }
            if (!string.IsNullOrWhiteSpace(typeConfig.NeedFields) && !string.IsNullOrWhiteSpace(context.Request["Keyword"]))
            {
                string keyword = context.Request["Keyword"];
                foreach (var item in typeConfig.NeedFields.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(item)) keywords.Add(item, keyword);
                }
            }
            int total = 0;
            string colName = typeConfig.Ex3;
            if (!colName.Contains("Sort")) colName = "Sort," + colName;
            if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
            {
                if (!colName.Contains("UserLongitude")) colName = "UserLongitude," + colName;
                if (!colName.Contains("UserLatitude")) colName = "UserLatitude," + colName;
            }
            if (!colName.Contains("JuActivityID")) colName = "JuActivityID," + colName;
            List<JuActivityInfo> list = bll.GetCommOutletsList(rows, page, bll.WebsiteOwner, type, false, typeConfig.TimeSetMethod, equals, contains, keywords, null, null, null, colName, out total, onlyLngLatIsNull);
            apiResp.result = new {
                totalcount = total,
                list = list
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}