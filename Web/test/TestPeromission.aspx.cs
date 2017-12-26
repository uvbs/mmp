using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Test
{
    public partial class TestPeromission : System.Web.UI.Page
    {
        protected bool showButton = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLPermission.BLLMenuPermission bllPer = new BLLPermission.BLLMenuPermission("");
            BLLPermission.Model.UserInfo curUser = bllPer.GetCurrentUserInfo();
            showButton = bllPer.CheckUserAndPmsKey(curUser.UserID, BLLPermission.Enums.PermissionSysKey.OnlineDistribution);
        }
    }
}