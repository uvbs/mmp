using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SystemConfig
{
    /// <summary>
    /// 商品扩展字段
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 表字段
        /// </summary>
        BLLJIMP.BLLTableFieldMap bllTabeFieldMap = new BLLJIMP.BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            List<TableFieldMapping> tableMapList = bllTabeFieldMap.GetTableFieldMapByTableName(bllTabeFieldMap.WebsiteOwner, "ZCJ_WXMallProductInfo");
            List<product> productList = new List<product>();
            var result = tableMapList.Select(p => new { ex_article_title = p.MappingName, ex_field = GetProductExFieldRename(p.Field) }).ToList();
            apiResp.result = new
            {
                product_ex_content = result
            };
            apiResp.msg = "查询完成";
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            
        }
        private string GetProductExFieldRename(string field)
        {
            string result = string.Empty;
            switch (field)
            {
                case "ExArticleTitle_1":
                    result = "ex_article_title_1";
                    break;
                case "ExArticleTitle_2":
                    result = "ex_article_title_2";
                    break;
                case "ExArticleTitle_3":
                    result = "ex_article_title_3";
                    break;
                case "ExArticleTitle_4":
                    result = "ex_article_title_4";
                    break;
                case "ExArticleTitle_5":
                    result = "ex_article_title_5";
                    break;
                default:
                    break;
            }

            return result;
        }
        public class product
        {
            public string ex_article_title_1 { get; set; }
            public string ex_article_title_2 { get; set; }
            public string ex_article_title_3 { get; set; }
            public string ex_article_title_4 { get; set; }
            public string ex_article_title_5 { get; set; }
        }

       
    }
}