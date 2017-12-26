using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.CrowdFund.Category
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        { 


            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}' AND CategoryType='{1}' ", bll.WebsiteOwner, "crowdfund"));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" CategoryName like '%{0}%'", keyWord);
            }
            int totalCount = bll.GetCount<ArticleCategory>(sbWhere.ToString());
            var categoryData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in categoryData
                       select new
                       {
                           category_name = p.CategoryName,
                           category_id = p.AutoID
                       };

            var data = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
            return;
        }
        /// <summary>
        /// 分类
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 分类编号
            /// </summary>
            public int category_id { get; set; }
            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }
        }

    }
}