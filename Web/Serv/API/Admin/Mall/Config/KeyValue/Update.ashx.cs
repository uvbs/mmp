using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.KeyValue
{
    /// <summary>
    /// 商城自定义配置更新接口
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLKeyValueData bllKeyValueData = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string id=context.Request["id"];
            string key = context.Request["key"];
            string value = context.Request["value"];
            if (string.IsNullOrEmpty(id))
            {
                apiResp.msg = "id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            if (string.IsNullOrEmpty(key))
            {
                apiResp.msg = "key 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            if (string.IsNullOrEmpty(value))
            {
                apiResp.msg = "value 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            KeyVauleDataInfo model = bllKeyValueData.Get<KeyVauleDataInfo>(string.Format(" AutoID={0}", id));
            model.DataKey = key;
            model.DataValue = value;
            if (bllKeyValueData.Update(model, string.Format(" DataKey='{0}',DataValue='{1}'", model.DataKey, model.DataValue), string.Format(" AutoId={0}", model.AutoId)) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {

                apiResp.msg = "添加失败";

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}