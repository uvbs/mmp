using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// 权限栏目列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermissionColumn bllPermissionColumn = new BLLPermissionColumn();
        BLLMenuInfo bllMenu = new BLLMenuInfo();
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string websiteOwner = context.Request["website_owner"];
            string show_hide = context.Request["show_hide"];
            string max_level = context.Request["max_level"];
            bool showHide = show_hide == "1" ? true : false;
            int maxLevel = 9;
            if (string.IsNullOrWhiteSpace(max_level)) max_level = "9";
            int.TryParse(max_level, out maxLevel);
            List<BLLPermission.Model.PermissionColumn> list = new List<BLLPermission.Model.PermissionColumn>();
            if (currentUserInfo.UserType != 1 && currentUserInfo.UserID != websiteOwner&&(currentUserInfo.PermissionGroupID!=3))
            {
                list = bllPermissionColumn.GetColumnListByUser(currentUserInfo.UserID, websiteOwner, showHide);
            }
            else
            {
                list = bllPermissionColumn.GetColumnListByWebsiteOwner(websiteOwner, showHide);
            }
            List<BLLPermission.Model.PermissionColumn> showList = new List<BLLPermission.Model.PermissionColumn>();
            bllPermissionColumn.GetDataList(ref showList, list, 0, 0, 1, maxLevel);

            int total = showList.Count;
            if (showList.Count > pageSize)
            {
                showList = showList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            List<dynamic> result = new List<dynamic>();
            foreach (var item in showList)
            {
                var has_menu = false;
                var menu_list = bllMenu.GetMenuRelationList(item.PermissionColumnID.ToString(), 5);
                var menu_strs = "";
                if (menu_list.Count > 0)
                {
                    has_menu = true;
                    menu_strs = ZentCloud.Common.MyStringHelper.ListToStr(menu_list.Select(p=>p.MenuID).ToList(), "", ",");
                }
                var has_permission = false;
                var permission_list = bllPermission.GetPermissionRelationList(item.PermissionColumnID.ToString(), 2);
                var permission_strs = "";
                if (permission_list.Count > 0)
                {
                    has_permission = true;
                    permission_strs = ZentCloud.Common.MyStringHelper.ListToStr(permission_list.Select(p => p.PermissionID).ToList(), "", ",");
                }
                result.Add(new
                {
                    id = item.PermissionColumnID,
                    name = item.PermissionColumnName,
                    pre_id = item.PermissionColumnPreID,
                    base_id = item.PermissionColumnBaseID,
                    order_num = item.OrderNum,
                    is_hide = item.IsHide,
                    website_owner = item.WebsiteOwner,
                    has_menu = has_menu,
                    has_permission = has_permission,
                    menu_strs = menu_strs,
                    permission_strs = permission_strs
                });
            }
            var data = new
            {
                totalcount = total,
                list = result//列表
            };
            apiResp.result = data;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllPermissionColumn.ContextResponse(context, apiResp);
        }
    }
}