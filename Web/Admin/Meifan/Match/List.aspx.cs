using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Admin.Meifan.Match
{
    public partial class List : System.Web.UI.Page
    {
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 添加权限
        /// </summary>
        public bool PmsAdd = false;
        /// <summary>
        /// 编辑权限
        /// </summary>
        public bool PmsUpdate = false;
        /// <summary>
        /// 删除权限
        /// </summary>
        public bool PmsDelete = false;
        /// <summary>
        /// 启用/禁用权限
        /// </summary>
        public bool PmsEnable = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            PmsAdd = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.PMS_MFMATCH_ADD);

            PmsUpdate = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.PMS_MFMATCH_UPDATE);

            PmsDelete = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.PMS_MFMATCH_DELETE);

            PmsEnable = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.PMS_MFMATCH_ENABLE);
        }
    }
}