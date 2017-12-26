using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    /// CheckedMenuList 的摘要说明
    /// </summary>
    public class CheckedMenuList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        BLLPermission.BLLMenuInfo bllMenu = new BLLPermission.BLLMenuInfo();
        public void ProcessRequest(HttpContext context)
        {
            string relation_id = context.Request["relation_id"];
            List<long> menuId_list = new List<long>();
            if (!string.IsNullOrWhiteSpace(relation_id))
            {
                List<BLLPermission.Model.MenuRelationInfo> relList = bllMenu.GetMenuRelationList(context.Request["relation_id"], 5);
                if (relList.Count > 0) menuId_list = relList.Select(p => p.MenuID).ToList();
            }
            apiResp.result = menuId_list;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMenuPermission.ContextResponse(context, apiResp);
        }
    }
}