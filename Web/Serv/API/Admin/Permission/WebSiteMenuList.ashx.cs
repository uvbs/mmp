using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    ///站点菜单列表
    /// </summary>
    public class WebSiteMenuList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLMenuInfo bllMenu = new BLLPermission.BLLMenuInfo();
        public void ProcessRequest(HttpContext context)
        {
            string websiteowner = context.Request["websiteowner"];
            int ShowLevel = 3;
            if (currentUserInfo.WebsiteOwner == websiteowner) ShowLevel = 2;
            if (currentUserInfo.UserType == 1) ShowLevel = 1;
            List<BLLPermission.Model.MenuInfo> MenuList = bllMenu.GetWebsiteMenuList(websiteowner, ShowLevel, false, true);
            MenuList = bllMenu.NewCheckMenuRelationList(MenuList, websiteowner);
            MenuList = MenuList.Where(p => p.IsHide.Value == 0).ToList();

            List<MenuModel> list = new List<MenuModel>();
            foreach (BLLPermission.Model.MenuInfo item in MenuList.Where(p => p.PreID == 0).OrderBy(p => p.MenuSort))
            {
                var model = new MenuModel();
                model.menu_id = item.MenuID;
                model.menu_name = item.NodeName;
                list.Add(GetTree(model, MenuList, item.MenuID));
            }

            apiResp.result = list;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMenu.ContextResponse(context, apiResp);
        }
        /// <summary>
        /// 返回模型
        /// </summary>
        public class MenuModel
        {
            /// <summary>
            /// 菜单id
            /// </summary>
            public long menu_id { get; set; }
            /// <summary>
            /// 菜单名称
            /// </summary>
            public string menu_name { get; set; }
            /// <summary>
            /// 子节点
            /// </summary>
            public List<MenuModel> children { get; set; }
            /// <summary>
            /// 是否选中
            /// </summary>
            public bool menu_checked { get; set; }
        }

        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private MenuModel GetTree(MenuModel model, List<BLLPermission.Model.MenuInfo> list,
            long? parentID = 0, int nLevel = 1, int maxLevel = 2)
        {
            if (model.children == null)
            {
                model.children = new List<MenuModel>();
            }
            if (nLevel <= maxLevel)
            {
                foreach (BLLPermission.Model.MenuInfo item in list.Where(p => p.PreID == parentID).OrderBy(p => p.MenuSort))
                {
                    MenuModel child = new MenuModel()
                    {
                        menu_id = item.MenuID,
                        menu_name = item.NodeName
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, item.MenuID, nLevel + 1);
                }
            }
            return model;
        }
    }
}