using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// ListSingle 的摘要说明
    /// </summary>
    public class ListSingle : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", bll.WebsiteOwner);
            if(context.Request["hide_page"] == "1") sbWhere.AppendFormat(" AND ComponentType != 'page' ");
            List<BLLJIMP.Model.Component> list = bll.GetColList<BLLJIMP.Model.Component>(int.MaxValue, 1, sbWhere.ToString(), "AutoId,ComponentName,ComponentType");
            apiResp.status = true;
            apiResp.result = from p in list
                             select new
                             {
                                 component_id = p.AutoId,
                                 component_name = p.ComponentName,
                                 component_type = p.ComponentType
                             };
            bll.ContextResponse(context, apiResp);
        }
    }
}