using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public class SelectRoleList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
        BLLPermission.BLLPermissionColumn bllPermissionColumn = new BLLPermission.BLLPermissionColumn();
        public void ProcessRequest(HttpContext context)
        {
            List<BLLPermission.Model.PermissionGroupInfo> list = bllPms.GetGroupList(int.MaxValue, 1, null, bllPms.WebsiteOwner, null, 2);
            list.AddRange(bllPms.GetGroupList(int.MaxValue, 1, "", "common", null, 3));
            list.AddRange(bllPms.GetGroupList(int.MaxValue, 1, "", "common", null, 4));
            List<ListItem> itemList = new List<ListItem>();
            if (currentUserInfo.UserType != 1 && currentUserInfo.UserID != bllPms.WebsiteOwner && !currentUserInfo.PermissionGroupID.HasValue)
            {
                List<BLLPermission.Model.PermissionGroupInfo> groupList = bllPms.GetUserGroupList(currentUserInfo.UserID, bllPms.WebsiteOwner, 2);
                if (groupList.Count > 0)
                {
                    foreach (BLLPermission.Model.PermissionGroupInfo item in groupList)
                    {
                        if (itemList.FirstOrDefault(p => p.Value == item.GroupID.ToString()) != null) continue;
                        bllPms.GetGroupItemList(ref itemList, list, item.GroupID, 1, 9, "");
                    }
                }
            }
            else
            {
                bllPms.GetGroupItemList(ref itemList, list, 0, 1, 9, "");
            }
            string result = new ZentCloud.Common.MyCategories().CreateSelectOptionHtml(itemList, "ddlPermissionGroup", "width: 140px", "");
            apiResp.result = result;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllPermissionColumn.ContextResponse(context, apiResp);
        }
    }
}