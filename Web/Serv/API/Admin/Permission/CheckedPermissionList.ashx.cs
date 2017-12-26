using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    /// CheckedPermissionList 的摘要说明
    /// </summary>
    public class CheckedPermissionList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string relation_id = context.Request["relation_id"];
            List<long> pmsId_list = new List<long>();
            if (!string.IsNullOrWhiteSpace(relation_id))
            {
                List<BLLPermission.Model.PermissionRelationInfo> relList = bllPermission.GetPermissionRelationList(relation_id, 2);
                if (relList.Count > 0) pmsId_list = relList.Select(p => p.PermissionID).ToList();
            }
            apiResp.result = pmsId_list;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMenuPermission.ContextResponse(context, apiResp);
        }

    }
}