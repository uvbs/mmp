using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Category
{
    /// <summary>
    /// Summary description for UpdateSort
    /// </summary>
    public class UpdateSort : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            int sort = Convert.ToInt32(context.Request["sort"]);

            int updateCount = BLLJIMP.BLLStatic.bll.Update(
                    new BLLJIMP.Model.WXMallCategory(),
                    string.Format(" Sort={0} ",sort),
                    string.Format(" AutoID IN ({0}) ", ids)
                );

            resp.isSuccess = updateCount > 0;
              
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        
    }
}