using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article.Category
{
    /// <summary>
    /// 文章分类列表接口
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLArticleCategory bll = new BLLJIMP.BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string type = context.Request["type"];
            if (string.IsNullOrWhiteSpace(type)) type = "Article";

            string websiteowner = context.Request["websiteowner"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bll.WebsiteOwner;
            int totalCount = 0;
            var mallCategoryData = bll.GetCateList(out totalCount, type, 0, websiteowner, pageSize, pageIndex, keyWord);
            var list = from p in mallCategoryData
                       select new
                       {
                           category_id = p.AutoID,
                           category_name = p.CategoryName,
                           category_summary = p.Summary,
                           category_img_url = p.ImgSrc,
                           category_sort = p.Sort,
                           category_systype = p.SysType,
                           category_parent_id = p.PreID
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
        }
    }
}