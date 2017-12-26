using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.User
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string keyWord = context.Request["keyWord"];

           StringBuilder sbWhere=new StringBuilder();
           sbWhere.AppendFormat(" WebsiteOwner='{0}' And DistributionOffLinePreUserId !=''", bllUser.WebsiteOwner);

           if (!string.IsNullOrEmpty(keyWord))
           {
               sbWhere.AppendFormat(" And (TrueName='{0}' Or convert(varchar,AutoId)='{0}')", keyWord);
           }
            int total = bllUser.GetCount<UserInfo>(sbWhere.ToString());
            var list = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoId Desc");
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(
                new
                {
                    total = total,
                    rows = list

                }
                ));

        }



    }
}