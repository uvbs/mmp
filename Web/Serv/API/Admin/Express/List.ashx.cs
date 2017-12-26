using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.ModelGen;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Express
{
    /// <summary>
    /// 快递公司列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        { 
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND ExpressCompanyName like '%{0}%'",keyWord);
            }
            int totalCount = bll.GetCount<ExpressInfo>(sbWhere.ToString());
            var expressData = bll.GetLit<ExpressInfo>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in expressData
                       select new
                       {
                           express_company_id=p.AutoID,
                           express_company_name =p.ExpressCompanyName,
                           express_company_code =p.ExpressCompanyCode
                           
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
            
        }

        public class RequestModel
        {
            /// <summary>
            /// 快递公司编号
            /// </summary>
            public int express_company_id { get; set; }

            /// <summary>
            /// 快递公司名称
            /// </summary>
            public string express_company_name { get; set; }

            /// <summary>
            /// 快递公司代码
            /// </summary>
            public string express_company_code { get; set; }


        }
    }
}