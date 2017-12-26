using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// SetHide 的摘要说明
    /// </summary>
    public class SetHide : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermissionColumn bllPermissionColumn = new BLLPermissionColumn();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string hide = context.Request["hide"];
            string websiteOwner = context.Request["websiteOwner"];

            List<string> id_list = ids.Split(',').ToList();
            int sCount = 0;
            foreach (string item in id_list)
            {
                SetHideColumn(ref sCount, item, hide, websiteOwner);
            }
            string str = hide == "1" ? "隐藏" : "显示";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = str + "[" + sCount + "]条栏目。";
            bllPermissionColumn.ContextResponse(context, apiResp);
        }
        private void SetHideColumn(ref int sCount, string id, string hide, string websiteOwner)
        {
            BLLPermission.Model.PermissionColumn nPermissionColumn = bllPermissionColumn.GetByKey<BLLPermission.Model.PermissionColumn>("PermissionColumnID", id);
            if (nPermissionColumn == null) return;
            bool isResult = false;
            if (string.IsNullOrWhiteSpace(websiteOwner) || websiteOwner == nPermissionColumn.WebsiteOwner)
            {
                isResult = bllPermissionColumn.UpdateByKey<BLLPermission.Model.PermissionColumn>("PermissionColumnID",
                        nPermissionColumn.PermissionColumnID.ToString(), "IsHide", hide) > 0;
            }
            else
            {
                isResult = bllPermissionColumn.AddColumn(nPermissionColumn.PermissionColumnName,
                    nPermissionColumn.PermissionColumnPreID,
                    nPermissionColumn.OrderNum,
                    websiteOwner,
                    nPermissionColumn.PermissionColumnID,
                    Convert.ToInt32(hide));
            }
            if (isResult) sCount++;
        }
    }
}