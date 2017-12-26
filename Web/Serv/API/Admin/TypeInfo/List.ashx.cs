using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TypeInfo
{
    /// <summary>
    /// 类型列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {


        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebsiteOwner='{0}'",bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND TypeName like '%{0}%'", keyWord);
            }
            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.TypeInfo>(sbWhere.ToString());
            var expressData = bll.GetLit<ZentCloud.BLLJIMP.Model.TypeInfo>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in expressData
                       select new
                       {
                           type_id = p.TypeId,
                           type_name = p.TypeName

                       };

            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }




    }
}