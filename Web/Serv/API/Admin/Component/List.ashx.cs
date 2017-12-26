using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string type = context.Request["type"];
            string name = context.Request["name"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1 ", bll.WebsiteOwner);
            sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", bll.WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(type) && type !="0")
            {
                sbWhere.AppendFormat(" AND ComponentType='{0}' ", type);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                sbWhere.AppendFormat(" AND ComponentName like '%{0}%' ", name);
            }
            int totalCount = bll.GetCount<BLLJIMP.Model.Component>(sbWhere.ToString());
            List<BLLJIMP.Model.Component> list = bll.GetLit<BLLJIMP.Model.Component>(rows, page, sbWhere.ToString());
            apiResp.status = true;
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in list)
            {
                ComponentModel nComponentModel = bll.GetByKey<ComponentModel>("AutoId", item.ComponentModelId.ToString());
                
                returnList.Add(new 
                {
                    component_id = item.AutoId,
                    component_name=item.ComponentName,
                    component_model_id = item.ComponentModelId,
                    component_model_name = nComponentModel == null ? "" : nComponentModel.ComponentModelName,
                    component_link_url = nComponentModel == null || string.IsNullOrWhiteSpace(nComponentModel.ComponentModelLinkUrl) ? "" : Regex.Replace(nComponentModel.ComponentModelLinkUrl, "{component_id}", item.AutoId.ToString(), RegexOptions.IgnoreCase),
                    child_component_ids=item.ChildComponentIds,
                    component_config=item.ComponentConfig,
                    decription=item.Decription,
                    component_key = item.ComponentKey,
                    access_level = item.AccessLevel
                });
            }
            apiResp.result = new 
            {
                totalcount=totalCount,
                list = returnList
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}