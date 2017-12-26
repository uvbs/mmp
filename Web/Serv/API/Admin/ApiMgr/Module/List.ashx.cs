using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ApiMgr.Module
{
    /// <summary>
    /// List 的摘要说明  获取api模块列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// api 业务逻辑层
        /// </summary>
        BLLJIMP.BLLApi bllApi = new BLLJIMP.BLLApi();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWords = context.Request["keyword"];
            string sort = context.Request["sort"];
            int totalCount = 0;
            List<ApiModule> ApiModuleList = bllApi.GetApiModuleList(pageSize, pageIndex, keyWords, sort,out totalCount);
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in ApiModuleList)
            {
                returnList.Add(new 
                {
                    module_id=item.AutoID,
                    module_name=item.ModuleName,
                    module_desc=item.Description
                });
            }
            apiResp.result = new 
            {
                totalcount=totalCount,
                list=returnList
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            bllApi.ContextResponse(context, apiResp);
        }
    }
}