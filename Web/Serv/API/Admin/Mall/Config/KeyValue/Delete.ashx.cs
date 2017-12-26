using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.KeyValue
{
    /// <summary>
    /// 商城自定义配置删除接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLKeyValueData bllKeyValueData = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                apiResp.msg = "ids 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            if (bllKeyValueData.Delete(new KeyVauleDataInfo(),string.Format(" AutoId in ({0}) And WebSiteOwner='{1}'",ids,bllKeyValueData.WebsiteOwner))==ids.Split(',').Length)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {

                apiResp.msg = "删除失败";

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}