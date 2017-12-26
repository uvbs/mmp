using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.KeyValue
{
    /// <summary>
    /// 商城自定义配置获取接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLKeyValueData bllKeyValueData = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                apiResp.msg = "id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            KeyVauleDataInfo model = bllKeyValueData.GetKeyData(int.Parse(id));
            var result = new
            {
                id = model.AutoId,
                key = model.DataKey,
                value = model.DataValue

            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = result;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}