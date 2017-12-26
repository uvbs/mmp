using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Menu
{
    /// <summary>
    /// 菜单列表接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMenuInfo bllMenu = new BLLMenuInfo();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                UserInfo currUser = bllMenu.GetCurrentUserInfo();
                List<MenuInfo> list = bllMenu.GetMenusByUser(currUser, bllMenu.WebsiteOwner);
                apiResp.result = GetResult(list);
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            catch (Exception ex)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = ex.Message;
            }
            bllMenu.ContextResponse(context, apiResp);
        }

        public object GetResult(List<MenuInfo> list)
        {
            List<dynamic> result = new List<dynamic>();
            foreach (MenuInfo item in list)
            {
                result.Add(new
                {
                    menu_id = item.MenuID,
                    menu_name = item.NodeName,
                    menu_url = item.Url,
                    menu_icon = item.ICOCSS,
                    show_level = item.ShowLevel,
                    menu_list = GetResult(item.ChildMenus)
                });
            }
            return result;
        }

    }
}