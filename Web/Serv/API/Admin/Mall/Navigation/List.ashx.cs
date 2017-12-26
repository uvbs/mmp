using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Navigation
{
    /// <summary>
    /// 导航列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string navigationType = context.Request["navigation_link_type"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND NavigationName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(navigationType))
            {
                sbWhere.AppendFormat(" AND NavigationLinkType ='{0}'", navigationType);
            }
            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.Navigation>(sbWhere.ToString());
            var navigationData = bll.GetLit<ZentCloud.BLLJIMP.Model.Navigation>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in navigationData
                       select new
                       {
                           navigation_id=p.AutoID,
                           pre_id=p.ParentId,
                           navigation_img=p.NavigationImage,
                           navigation_name=p.NavigationName,
                           navigation_link=p.NavigationLink,
                           navigation_link_type = p.NavigationLinkType,
                           navigation_sort=p.Sort

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