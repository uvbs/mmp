using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// CheckedPermissionColumnList 的摘要说明
    /// </summary>
    public class RoleCheckedPermissionColumnList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermissionColumn bllPermissionColumn = new BLLPermission.BLLPermissionColumn();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string group_id = context.Request["group_id"];
            List<long> relId_list = new List<long>();
            if (!string.IsNullOrWhiteSpace(group_id) && group_id != "0")
            {
                List<BLLPermission.Model.PermissionRelationInfo> relList = new List<BLLPermission.Model.PermissionRelationInfo>();
                relList = bllPermission.GetPermissionRelationList(group_id, 3);
                if (relList.Count > 0) relId_list =relList.Select(p => p.PermissionID).ToList();
            }
            apiResp.result = relId_list;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllPermissionColumn.ContextResponse(context, apiResp);
        }
    }
}