using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TableFieldMap
{
    /// <summary>
    /// 表映射列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            string table_name = context.Request["table_name"];
            string mapping_type = context.Request["mapping_type"];
            string foreignkey_id = context.Request["foreignkey_id"];
            List<TableFieldMapping> list = bll.GetTableFieldMap(bll.WebsiteOwner, table_name, foreignkey_id, null, false, mapping_type);
            if (list.Count == 0) list = bll.GetTableFieldMap(null, table_name, null, null, false, mapping_type);

            apiResp.result = new
            {
                totalcount = list.Count,
                list = list
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}