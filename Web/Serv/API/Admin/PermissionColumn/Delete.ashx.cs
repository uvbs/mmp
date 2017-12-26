using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// 删除权限栏目接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermissionColumn bllPermissionColumn = new BLLPermission.BLLPermissionColumn();
        BLLPermission.BLLMenuInfo bllMenu = new BLLPermission.BLLMenuInfo();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> id_list = ids.Split(',').ToList();
            int sCount = 0;
            foreach (string item in id_list)
            {
                DeleteColumn(ref sCount, item);
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "删除[" + sCount + "]条栏目。";
            bllPermissionColumn.ContextResponse(context, apiResp);
        }

        private void DeleteColumn(ref int sCount, string item)
        {

            if (bllPermissionColumn.DeleteByKey<BLLPermission.Model.PermissionColumn>("PermissionColumnID", item) > 0)
            {
                List<BLLPermission.Model.PermissionColumn> childrenList = bllPermissionColumn.GetMultListByKey<BLLPermission.Model.PermissionColumn>("PermissionColumnPreID", item);
                foreach (var child in childrenList)
                {
                    DeleteColumn(ref sCount, child.PermissionColumnID.ToString());
                }
                List<BLLPermission.Model.PermissionColumn> childcopyList = bllPermissionColumn.GetMultListByKey<BLLPermission.Model.PermissionColumn>("PermissionColumnBaseID", item);
                foreach (var child in childcopyList)
                {
                    DeleteColumn(ref sCount, child.PermissionColumnID.ToString());
                }
                bllMenu.DeleteMenuRelation(item, 5);
                bllPermission.DeletePermissionRelation(item, 2);
                bllPermission.DeletePermissionRelationByPermission(item, 3);
                sCount++;
            }
        }
    }
}