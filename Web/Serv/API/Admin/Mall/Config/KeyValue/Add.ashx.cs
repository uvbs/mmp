using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.KeyValue
{
    /// <summary>
    /// 商城自定义配置添加接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLKeyValueData bllKeyValueData = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request["key"];
            string value=context.Request["value"];
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
            KeyVauleDataInfo model = new KeyVauleDataInfo();
            model.WebsiteOwner = bllKeyValueData.WebsiteOwner;
            model.CreateTime = DateTime.Now;
            model.DataType = "MallConfig";
            model.DataKey = key;
            model.DataValue = value;
            if (bllKeyValueData.Add(model))
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