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
    /// 权限栏目下拉框
    /// </summary>
    public class SelectList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermissionColumn bllPermissionColumn = new BLLPermissionColumn();
        public void ProcessRequest(HttpContext context)
        {
            string websiteOwner = context.Request["website_owner"];
            string show_hide = context.Request["show_hide"];
            string max_level = context.Request["max_level"];
            bool showHide = false;
            bool.TryParse(show_hide, out showHide);
            int maxLevel = 2;
            if (string.IsNullOrWhiteSpace(max_level)) max_level = "2";
            int.TryParse(max_level, out maxLevel);

            List<BLLPermission.Model.PermissionColumn> list = bllPermissionColumn.GetColumnListByWebsiteOwner(websiteOwner, showHide);

            //判断版本
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {

            }
            string result = string.Empty;
            List<ListItem> itemList = new List<ListItem>();
            bllPermissionColumn.GetItemList(ref itemList, list, 0, 0, 1, maxLevel, "");
            result = new ZentCloud.Common.MyCategories().CreateSelectOptionHtml(itemList, "ddlPermissionColumn", "width:90%", "");
            apiResp.result = result;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllPermissionColumn.ContextResponse(context, apiResp);
        }

    }
}