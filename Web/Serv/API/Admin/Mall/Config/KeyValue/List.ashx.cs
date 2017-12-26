using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.KeyValue
{
    /// <summary>
    /// 商城自定义配置列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLKeyValueData bllKeyValueData = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount = 0;
            List<KeyVauleDataInfo> sourceList = bllKeyValueData.GetKeyVauleDataInfoList(pageSize, pageIndex, "MallConfig", "", bllKeyValueData.WebsiteOwner, out  totalCount);
            var list = from p in sourceList
                       select new
                       {
                           id = p.AutoId,
                           key = p.DataKey,
                           value = p.DataValue
                       };
            var result = new
            {
                totalcount = totalCount,
                list = list

            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = result;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}