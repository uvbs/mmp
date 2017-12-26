using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.WebPC
{
    /// <summary>
    /// 页面列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = string.IsNullOrEmpty(context.Request["page"]) ? 1 : int.Parse(context.Request["page"]);
            int pageSize = string.IsNullOrEmpty(context.Request["rows"]) ? 1 : int.Parse(context.Request["rows"]);
            string keyWord=context.Request["keyword"];

            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();

            sbWhere.AppendFormat(" WebsiteOwner='{0}'",bll.WebsiteOwner);

            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And PageName like '%{0}%'",keyWord);


            }
            var totalCount=bll.GetCount<PcPage>(sbWhere.ToString());
            var sourceData = bll.GetLit<PcPage>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in sourceData
                       select new { 
                       p.PageId,
                       p.PageName
                       
                       };

            var data = new
            {
                total=totalCount,
                rows=list

            };

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));


        }

        
    }
}