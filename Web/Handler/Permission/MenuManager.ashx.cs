using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using ZentCloud.ZCBLLEngine;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Handler.Permission
{
    /// <summary>
    /// MenuManager 的摘要说明
    /// </summary>
    public class MenuManager : IHttpHandler, IRequiresSessionState
    {
        BLLMenuPermission pmsBll = new BLLMenuPermission("");
        UserInfo currentUserInfo = new UserInfo();
        BLLMenuInfo bllMenu = new BLLMenuInfo();
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            currentUserInfo = pmsBll.GetCurrentUserInfo();
            string result = "true";
            switch (action)
            {
                case "Add":
                    result = Add(context);
                    break;
                case "Edit":
                    result = Edit(context);
                    break;
                case "Delete":
                    result = Delete(context);
                    break;
                case "Query":
                    result = Query(context);
                    break;
                case "GetMenuSelectList":
                    result = GetMenuSelectList(context);
                    break;
                case "CopyBaseMenu":
                    result = CopyBaseMenu(context);
                    break;
                case "HideWebsiteMenu":
                    result = HideWebsiteMenu(context);
                    break;
                case "ShowWebsiteMenu":
                    result = ShowWebsiteMenu(context);
                    break;
                case "EditBaseMenu":
                    result = EditBaseMenu(context);
                    break;
                    
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 获取菜单选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMenuSelectList(HttpContext context)
        {
            List<MenuInfo> list = new List<MenuInfo>();
            string websiteOwner = context.Request["websiteOwner"];
            string menuTypeStr = context.Request["menuType"];
            string showPreMenuStr = context.Request["showPreMenu"];
            int menuType = string.IsNullOrWhiteSpace(menuTypeStr) ? 3 : Convert.ToInt32(menuTypeStr);
            bool showPreMenu = showPreMenuStr == "1" ? true : false;
            string showLevelStr = context.Request["showLevel"];
            string showHideStr = context.Request["showHide"];
            bool showHide = showHideStr == "1" ? true : false;
            int showLevel = 3;
            if (currentUserInfo.UserType == 1)
            {
                showLevel =  showLevelStr == "1" ? 1 : 2;
            }
            else if (currentUserInfo.UserID == pmsBll.WebsiteOwner)
            {
                showLevel = 2;
            }

            list = bllMenu.GetWebsiteMenuList(websiteOwner, showLevel, showPreMenu, showHide);

            //获取权限组ids
            string relationIds = "''";
            List<long> pmsGroupIdList = pmsBll.GetPmsGroupIDByUser(websiteOwner);
            if (pmsGroupIdList.Count > 0) relationIds = MyStringHelper.ListToStr(pmsGroupIdList, "'", ",");

            list = bllMenu.CheckMenuRelationList(list, websiteOwner, relationIds);
            if (!showHide) list = list.Where(p => p.IsHide.Value == 0).ToList();

            string result = string.Empty;
            result = new MySpider.MyCategories().GetSelectOptionHtml(list, "MenuID", "PreID", "NodeName", 0, "ddlPreMenu", "width:90%", "");
            return result.ToString();
        }

        private string Delete(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_DeleteMenu))
            //{
            //    return "无权删除菜单";
            //}
            //#endregion

            string ids = context.Request["ids"];
            string websiteOwner = context.Request["websiteOwner"];

            //TODO:删除菜单前，清除相关权限菜单关联
            int result = bllMenu.DeleteMenu(ids, websiteOwner);//pmsBll.DeleteUser(idsList);
            return result.ToString();
        }

        private string Add(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_AddMenu))
            //{
            //    return "无权添加菜单";
            //}
            //#endregion
            try
            {
                string jsonData = context.Request["JsonData"];
                MenuInfo menuInfo = ZentCloud.Common.JSONHelper.JsonToModel<MenuInfo>(jsonData);
                menuInfo.MenuID = long.Parse(pmsBll.GetGUID(Common.TransacType.MenuAdd));
                bool result = pmsBll.Add(menuInfo);
                return result.ToString().ToLower();
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

        }

        private string Edit(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_EditMenu))
            //{
            //    return "无权修改菜单信息";
            //}
            //#endregion

            string jsonData = context.Request["JsonData"];
            MenuInfo menuInfo = ZentCloud.Common.JSONHelper.JsonToModel<MenuInfo>(jsonData);

            MenuInfo menuOld = pmsBll.Get<MenuInfo>(string.Format(" MenuID={0} ", menuInfo.MenuID));
            bool result = pmsBll.Update(menuInfo);
            if (result && menuInfo.ShowLevel != menuOld.ShowLevel) pmsBll.UpdateChildShowLevel(menuInfo.MenuID, menuInfo.ShowLevel);
            return result.ToString().ToLower();
        }

        private string Query(HttpContext context)
        {
            //int page = Convert.ToInt32(context.Request["page"]);
            //int rows = Convert.ToInt32(context.Request["rows"]);

            List<MenuInfo> menuList = new List<MenuInfo>();
            
            //分页去掉 
            //list = pmsBll.GetLit<MenuInfo>(rows, page, null, "MenuID");
            string websiteOwner = context.Request["websiteOwner"];
            string menuTypeStr = context.Request["menuType"];
            int menuType = string.IsNullOrWhiteSpace(menuTypeStr) ? 3 : Convert.ToInt32(menuTypeStr);
            string showLevelStr = context.Request["showLevel"];
            string showHideStr = context.Request["showHide"];
            bool showHide = showHideStr == "1" ? true : false;
            int showLevel = 3;
            if (currentUserInfo.UserType == 1)
            {
                showLevel = showLevelStr == "1" ? 1 : 2;
            }
            else if (currentUserInfo.UserID == pmsBll.WebsiteOwner)
            {
                showLevel = 2;
            }
            menuList.AddRange(bllMenu.GetWebsiteMenuList(websiteOwner, showLevel, false, showHide));

            //获取权限组ids
            string relationIds = "''";
            List<long> pmsGroupIdList = pmsBll.GetPmsGroupIDByUser(websiteOwner);
            if (pmsGroupIdList.Count > 0) relationIds = MyStringHelper.ListToStr(pmsGroupIdList, "'", ",");

            menuList = bllMenu.CheckMenuRelationList(menuList, websiteOwner, relationIds);

            if (!showHide) menuList = menuList.Where(p=>p.IsHide.Value == 0).ToList();

            List<MenuInfo> showList = new List<MenuInfo>();

            MySpider.MyCategories m = new MySpider.MyCategories();
            List<MySpider.Model.MyCategoryModel> listCate = m.GetCommCateModelList<MenuInfo>("MenuID", "PreID", "NodeName", menuList);
            foreach (ListItem item in m.GetCateListItem(listCate, 0))
            {
                try
                {
                    MenuInfo tmpModel = (MenuInfo)menuList.Where(p => p.MenuID.ToString().Equals(item.Value)).ToList()[0].Clone();
                    tmpModel.NodeName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }
            int totalCount = showList.Count;
            return Common.JSONHelper.ObjectToJson(
                new{
                    total = totalCount,
                    rows = showList
                });
      }
        private string CopyBaseMenu(HttpContext context)
        {
            //string websiteOwner = context.Request["websiteOwner"];
            //string menuIds = context.Request["menuIds"];
            //List<MenuInfo> list = bllMenu.GetWebsiteMenuList("", 1, 2);
            //if (list.Count == 0) return "false";
            //BLLTransaction tran = new BLLTransaction();
            //string result = "";
            //try
            //{
            //    pmsBll.DeleteMenu(2, websiteOwner, null);
            //    if(pmsBll.CopyBaseMenu(list, websiteOwner, 2, 0, 0, tran))
            //    {
            //        result = "true";
            //        tran.Commit();
            //    }
            //    else
            //    {
            //        result = "false";
            //        tran.Rollback();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = "false";
            //    tran.Rollback();
            //    throw ex;
            //}
            return "false";
        }
        private string HideWebsiteMenu(HttpContext context)
        {
            string ids = context.Request["ids"];
            string websiteOwner = context.Request["websiteOwner"];
            int result = bllMenu.WebsiteHideMenu(ids, websiteOwner);
            return result.ToString();
        }
        private string ShowWebsiteMenu(HttpContext context)
        {
            string ids = context.Request["ids"];
            string websiteOwner = context.Request["websiteOwner"];
            int result = bllMenu.WebsiteShowMenu(ids, websiteOwner);
            return result.ToString();
        }
        /// <summary>
        /// 编辑基本菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditBaseMenu(HttpContext context)
        {
            string websiteOwner = context.Request["website_owner"];
            long menuID = Convert.ToInt64(context.Request["menu_id"]);
            string showLevel = context.Request["show_level"];
            string menuSort = context.Request["menu_sort"];
            string menuName = context.Request["menu_name"];
            if (bllMenu.EditBaseMenuEx(websiteOwner, menuID, showLevel, 2) == -1 ||
                bllMenu.EditBaseMenuEx(websiteOwner, menuID, menuSort, 3) == -1 ||
                bllMenu.EditBaseMenuEx(websiteOwner, menuID, menuName, 4) == -1)
            {
                return "false";
            }
            return "true";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    
    }

}