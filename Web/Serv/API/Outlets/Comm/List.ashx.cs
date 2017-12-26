using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Outlets.Comm
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            Dictionary<string, string> equals = new Dictionary<string, string>();
            Dictionary<string, string> contains = new Dictionary<string, string>();
            Dictionary<string, string> keywords = new Dictionary<string, string>();

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string type = context.Request["type"];
            ArticleCategoryTypeConfig typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, type);

            if (!string.IsNullOrWhiteSpace(typeConfig.Ex1))
            {
                foreach (var item in typeConfig.Ex1.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(context.Request[item])) equals.Add(item, context.Request[item]);
                }
            }
            if (!string.IsNullOrWhiteSpace(typeConfig.Ex2) && !string.IsNullOrWhiteSpace(context.Request["keyword"]))
            {
                string keyword = context.Request["keyword"];
                foreach (var item in typeConfig.Ex2.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(item)) keywords.Add(item, keyword);
                }
            }

            int total = 0;
            string colName = typeConfig.Ex4;
            if (!colName.Contains("JuActivityID")) colName = "JuActivityID," + colName;
            if (!colName.Contains("Sort")) colName = colName+",Sort";
            if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2) colName = "UserLongitude,UserLatitude," + colName;
            colName = colName.TrimStart(',').TrimEnd(',');
            List<JuActivityInfo> list = bll.GetCommOutletsList(rows, page, bll.WebsiteOwner, type, false, typeConfig.TimeSetMethod, equals, contains, keywords, context.Request["longitude"], context.Request["latitude"], null, colName, out total);

            JArray jrList = new JArray();
            if (list.Count > 0)
            {
                if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2) colName = colName + ",Distance";
                JArray jList = JArray.FromObject(list);
                List<string> colFieldList = colName.Split(',').ToList();
                foreach (JToken item in jList)
                {
                    JToken jr = new JObject();
                    foreach (var field in colFieldList)
                    {
                        jr[field] = item[field];
                    }
                    jrList.Add(jr);
                }
            }
            apiResp.result = new
            {
                totalcount = total,
                list = jrList
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
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