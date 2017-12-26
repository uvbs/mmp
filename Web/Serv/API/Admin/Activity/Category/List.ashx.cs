using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.Category
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}' AND CategoryType='activity'", bll.WebsiteOwner));
            if (string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND CategoryName like '%{0}%' ", keyWord);
            }
            int totalCount = bll.GetCount<ArticleCategory>(sbWhere.ToString());
            var mallCategoryData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in mallCategoryData
                       select new
                       {
                           category_id = p.AutoID,
                           category_name = p.CategoryName,
                           category_summary=p.Summary,
                           category_img_url=p.ImgSrc,
                           category_sort=p.Sort,
                           category_systype=p.SysType,
                           category_parent_id=p.PreID
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