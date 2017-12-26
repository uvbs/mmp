using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.ModelGen;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public  void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND LinkText like '%{0}%'", keyWord);
            }
            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.Slide>(sbWhere.ToString());
            var slideData = bll.GetLit<ZentCloud.BLLJIMP.Model.Slide>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in slideData
                       select new
                       {
                           slide_id=p.AutoID,
                           img_url=p.ImageUrl,
                           link=p.Link,
                           link_text=p.LinkText,
                           slide_type=p.Type,
                           slide_sort=p.Sort
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