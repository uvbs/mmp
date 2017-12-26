using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class MenuManagerV2 : System.Web.UI.Page
    {
        /// <summary>
        /// 添加菜单权限
        /// </summary>
        public bool isAddMenu=true;

        /// <summary>
        /// 编辑菜单权限
        /// </summary>
        public bool isEditMenu=true;

        /// <summary>
        /// 删除菜单权限
        /// </summary>
        public bool isDeleteMenu=true;

        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected UserInfo CurrentUserInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUserInfo = (new BLLJIMP.BLL()).GetCurrentUserInfo();
            //isAddMenu = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_AddMenu);
            //isEditMenu = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_EditMenu);
            //isDeleteMenu = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_DeleteMenu);


        }
    }
}