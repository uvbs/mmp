using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Model
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        List<string> limitControls = BLLComponent.limitControls;
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string show_delete = context.Request["show_delete"];
            bool showDelete = false;
            if(show_delete == "1") showDelete=true;
            int total = 0;
            List<ComponentModel> list = bll.GetComponentModelList(rows, page, context.Request["keyword"], out total, context.Request["component_model_type"], showDelete);

            List<ComponentModelField> componentModelFields = new List<ComponentModelField>();
            if (list.Count > 0)
            {
                string cpKeys = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p => p.ComponentModelKey).Distinct().ToList(), "'", ",");
                componentModelFields = bll.GetMultListByKey<ComponentModelField>("ComponentModelKey", cpKeys).OrderBy(p => p.ComponentFieldSort).ToList();
            }
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            List<dynamic> result = new List<dynamic>();
            foreach (var item in list)
            {
                KeyVauleDataInfo keydata = bllKeyValueData.GetKeyData("ComponentType", item.ComponentModelType, "Common");
                result.Add(new
                {
                    component_model_id = item.AutoId,
                    component_model_key = item.ComponentModelKey,
                    component_model_name = item.ComponentModelName,
                    is_delete = item.IsDelete,
                    component_model_fields = (from p in componentModelFields.Where(p => p.ComponentModelKey == item.ComponentModelKey && p.ComponentFieldType >= 4 && p.ComponentFieldType != 8 && limitControls.Contains(p.ComponentField))
                                              select new
                                              {
                                                  component_field_id = p.AutoId,
                                                  component_field = p.ComponentField,
                                                  component_field_name = p.ComponentFieldName,
                                                  component_field_type = p.ComponentFieldType,
                                                  component_field_data_value = p.ComponentFieldDataValue,
                                                  disabled = false
                                              })
                });
            }
            apiResp.result =new 
                {
                    totalcount = total,
                    list = result
                };
            bll.ContextResponse(context, apiResp);
        }

    }
}